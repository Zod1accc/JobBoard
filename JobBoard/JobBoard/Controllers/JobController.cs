using JobBoard.Helpers;
using JobBoard.Models;
using JobBoard.Models.DataContext;
using JobBoard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JobBoard.Controllers
{
	public class JobController : Controller
	{
		private DataContext _dataContext;
		private UserManager<AppUser> _userManager;
		private IWebHostEnvironment _env;

		public JobController(DataContext dataContext,UserManager<AppUser> userManager,IWebHostEnvironment env)
        {
			_dataContext = dataContext;
			_userManager = userManager;
			_env = env;
        }
		[NonAction]
        public IActionResult Index()
		{
			return View();
		}

		//job CRUD start

		[Authorize(Roles = "Company")]
		public IActionResult CreateJob()
		{
			ViewBag.Types = _dataContext.JobTypes.ToList();
			ViewBag.Genres = _dataContext.Genders.ToList();

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateJob(JobViewModel jobVM)
		{
			ViewBag.Types = _dataContext.JobTypes.ToList();
			ViewBag.Genres = _dataContext.Genders.ToList();

			if (!ModelState.IsValid) return View(jobVM);

			AppUser user = null;

			if (jobVM.ImageFile is null)
			{
				ModelState.AddModelError("ImageFile", "Image File is required!");
				return View(jobVM);
			}

			JobDetail newJob = TypeConvert.Converter<JobViewModel,JobDetail>(jobVM);

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(User.Identity.Name);
			}
			var company = _dataContext.Companies.FirstOrDefault(x => x.AppUserId == user.Id);
			if (company is null) return NotFound();

			newJob.Company = company;
			newJob.PublishedOn = DateTime.Now;


			if (jobVM.ImageFile.ContentType != "image/png" && jobVM.ImageFile.ContentType != "image/jpeg" && jobVM.ImageFile.ContentType != "image/jpg")
			{
				ModelState.AddModelError("ImageFile", "Yalniz Shekil fayli ola biler!");
				return View(jobVM);
			}
			if (jobVM.ImageFile.Length > 3145728)
			{
				ModelState.AddModelError("ImageFile", "Faylin ölçüsü max 3 mb ola biler!");
				return View(jobVM);
			}

			newJob.Image = FileManager.FileSave(_env.WebRootPath, "uploads\\jobs", jobVM.ImageFile);

			_dataContext.Jobs.Add(newJob);
			_dataContext.SaveChanges();

			return RedirectToAction("Index", "Home");
		}

		[Authorize(Roles = "Company")]
		public async Task<IActionResult> JobEdit(int id)
		{
			ViewBag.Types = _dataContext.JobTypes.ToList();
			ViewBag.Genres = _dataContext.Genders.ToList();

			var job = _dataContext.Jobs
							.Include(x => x.Company)
								.ThenInclude(c => c.AppUser)
							.Include(x => x.JobType)
							.Include(x => x.Gender)
							.FirstOrDefault(x => x.Id == id);
			if (job == null) return NotFound();

			AppUser user = null;

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(User.Identity.Name);
			}

			if (job.Company.AppUserId != user?.Id) return NotFound();

			JobEditViewModel jobEditVM = TypeConvert.Converter<JobDetail, JobEditViewModel>(job);

			return View(jobEditVM);
		}

		[HttpPost]
		public IActionResult JobEdit(JobEditViewModel jobEditVM)
		{
			ViewBag.Types = _dataContext.JobTypes.ToList();
			ViewBag.Genres = _dataContext.Genders.ToList();

			if (!ModelState.IsValid) return View(jobEditVM);

			var existJob = _dataContext.Jobs.FirstOrDefault(x => x.Id == jobEditVM.Id);
			if (existJob == null) return NotFound();

			if (jobEditVM.ImageFile is not null)
			{
				if (jobEditVM.ImageFile.ContentType != "image/png" && jobEditVM.ImageFile.ContentType != "image/jpeg" && jobEditVM.ImageFile.ContentType != "image/jpg")
				{
					ModelState.AddModelError("ImageFile", "Yalniz Shekil fayli ola biler!");
					return View(jobEditVM);
				}
				if (jobEditVM.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "Faylin ölçüsü max 3 mb ola biler!");
					return View(jobEditVM);
				}

				FileManager.FileDelete(_env.WebRootPath, "uploads\\jobs", existJob.Image);

				existJob.Image = FileManager.FileSave(_env.WebRootPath, "uploads\\jobs", jobEditVM.ImageFile);
			}

			existJob = TypeConvert.Converter<JobEditViewModel, JobDetail>(jobEditVM, existJob);

			_dataContext.SaveChanges();

			return RedirectToAction(nameof(JobList));
		}

		//job CRUD end />

		[Authorize(Roles ="Company")]
		public async Task<IActionResult> JobList(string? searchString,int? jobtype)
		{
			ViewBag.JobTypes = _dataContext.JobTypes.ToList();
			AppUser user = null;

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(User.Identity.Name);
			}

			var company = _dataContext.Companies.FirstOrDefault(x => x.AppUserId == user.Id);
			if (company == null) return NotFound();

			var jobs = _dataContext.Jobs
							.Include(x => x.Company)
							.Include(x => x.JobType)
							.AsEnumerable()
							.Where(x=>x.CompanyId == company.Id).AsQueryable();

			if(searchString is not null && jobtype is not null)
			{
				string[] keywords = searchString.ToLower().Split(' ');

				jobs = jobs.Where(x => keywords.All(k => x.JobTitle.ToLower().Contains(k)) || x.JobTypeId == jobtype);
			}
			else
			{
				if (searchString is not null)
				{
					string[] keywords = searchString.ToLower().Split(' ');

					jobs = jobs.Where(x => keywords.All(k => x.JobTitle.ToLower().Contains(k)));
				}

				if (jobtype is not null) jobs = jobs.Where(x => x.JobTypeId == jobtype);
			}
			


			JobListViewModel jobListVM = new JobListViewModel
			{
				jobs = jobs.ToList()
			};

			return View(jobListVM);
		}

		
		public async Task<IActionResult> JobDetail(int id)
		{
			JobDetail Job = _dataContext.Jobs
									.Include(x => x.Company)
										.ThenInclude(x => x.AppUser)
									.Include(x => x.JobType)
									.Include(x=>x.Gender)
									.AsEnumerable()
									.FirstOrDefault(x => x.Id == id);

			if (Job == null) return NotFound();

			AppUser user = null;

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(User.Identity.Name);
			}

			//related jobs

			string[] titleKeyvords = Job.JobTitle.ToLower().Split(' ');

			var query = _dataContext.Jobs
							.Include(x=>x.Company)
							.Include(x=>x.JobType)
							.Include(x=>x.Gender)
							.Where(x=>x.Id != Job.Id)
							.AsQueryable();

			var relatedJobs = query.AsEnumerable().Where(x=>x.JobType.Id == Job.JobType.Id || titleKeyvords.All(k=>x.JobTitle.ToLower().Contains(k)));

			JobDetailViewModel jobDetailVM = new JobDetailViewModel
			{
				job = Job,
				check = Job.Company.AppUserId == user?.Id ? true : false,
				relatedJobs = relatedJobs.ToList()
			};

			return View(jobDetailVM);
		
		}

		public IActionResult Jobs()
		{
			ViewBag.JobTypes = _dataContext.JobTypes.ToList();

			var jobs = _dataContext.Jobs
							.Include(x=>x.Company)
							.Include(x=>x.JobType)
							.Include(x=>x.Gender)
							.ToList();

			JobsVM model = new JobsVM
			{
				Jobs = jobs
			};

			return View(model);
		}

		[HttpPost]
		public IActionResult Jobs(string? searchStr,string? locationStr,int? jobTypeId)
		{
			ViewBag.JobTypes = _dataContext.JobTypes.ToList();

			var query = _dataContext.Jobs
							.Include(x=>x.Company)
							.Include(x=>x.JobType)
							.Include(x=>x.Gender)
							.AsQueryable();

			string[] titleKeywords = null;
			string[] locatoinKeywords = null;

			if (searchStr != null) titleKeywords = searchStr.ToLower().Split(' ');
            if(locationStr != null) locatoinKeywords = locationStr.ToLower().Split(' ');

			List<JobDetail> jobs = query.ToList();

			if (searchStr != null) jobs = jobs.Where(x => titleKeywords.All(k => x.JobTitle.ToLower().Contains(k) || x.Company.Name.ToLower().Contains(k))).ToList();
			if(locationStr != null) jobs = jobs.Where(x => locatoinKeywords.All(k => x.Location.ToLower().Contains(k))).ToList();
			if(jobTypeId != null) jobs = jobs.Where(x => x.JobTypeId == jobTypeId).ToList();

			ViewBag.SearchStr = searchStr;
			ViewBag.LocationStr = locationStr;

			JobsVM model = new JobsVM
			{
				Jobs = jobs
			};

            #region Filter
            //List<JobDetail> jobsForTitle = null;
            //List<JobDetail> jobsForLocation = null;
            //List<JobDetail> jobsForType = null;
            //if(searchStr != null)
            //{
            //	string[] titleKeywords = searchStr.ToLower().Split(' ');
            //	jobsForTitle = query.Where(x=>titleKeywords.All(k=>x.JobTitle.ToLower().Contains(k) || x.Company.Name.ToLower().Contains(k))).ToList();
            //}
            //if(locationStr != null)
            //{
            //	string[] locatoinKeywords = locationStr.ToLower().Split(' ');
            //	jobsForLocation = query.Where(x => locatoinKeywords.All(k => x.Location.ToLower().Contains(k))).ToList();
            //}
            //if(jobTypeId != null)
            //{
            //	jobsForType = query.Where(x=>x.JobTypeId == jobTypeId).ToList();
            //}
            #endregion


            return View(model);
		}

		public async Task<IActionResult> AddFavourite(int id)
		{
			if (!_dataContext.Jobs.Any(x => x.Id == id)) return NotFound();

			List<FavouriteViewModel> favouriteItems = new List<FavouriteViewModel>();
			FavouriteViewModel favouriteItem = null;
			List<Favourite> userFavourites = new List<Favourite>();
			Favourite userFavourite = null;
			int statusCode = 0;
			AppUser user = null;

			var job = _dataContext.Jobs.FirstOrDefault(x => x.Id == id);
			if (job == null) return NotFound();

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(User.Identity.Name);
			}

			if(user == null)
			{
				string favouriteItemsStr = HttpContext.Request.Cookies["Favourite"];

				if(favouriteItemsStr != null)
				{
					favouriteItems = JsonConvert.DeserializeObject<List<FavouriteViewModel>>(favouriteItemsStr);

					if (!favouriteItems.Any(x => x.JobId == id))
					{
						favouriteItem = new FavouriteViewModel
						{
							JobId = job.Id,
						};

						favouriteItems.Add(favouriteItem);
						statusCode = 200;
					}
					else
					{
						favouriteItem = favouriteItems.FirstOrDefault(x => x.JobId == id);
						favouriteItems.Remove(favouriteItem);
						statusCode = 201;
					}
				}
				else
				{
					favouriteItem = new FavouriteViewModel
					{
						JobId = job.Id,
					};

					favouriteItems.Add(favouriteItem);
					statusCode = 200;
				}

				favouriteItemsStr = JsonConvert.SerializeObject(favouriteItems);

				HttpContext.Response.Cookies.Append("Favourite", favouriteItemsStr);
			}
			else
			{
				userFavourite = _dataContext.Favourites.FirstOrDefault(x => x.AppUserId == user.Id && x.JobDetailId == id);

				if (userFavourite == null)
				{
					userFavourite = new Favourite
					{
						AppUserId = user.Id,
						JobDetailId = job.Id,
					};

					_dataContext.Favourites.Add(userFavourite);
					statusCode = 200;
				}
				else
				{
					_dataContext.Favourites.Remove(userFavourite);
					statusCode = 201;
				}
				_dataContext.SaveChanges();
			}

			return Json(new { status = statusCode });

		}
	}
}
