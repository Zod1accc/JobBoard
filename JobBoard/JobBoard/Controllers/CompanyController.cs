using JobBoard.Helpers;
using JobBoard.Models;
using JobBoard.Models.DataContext;
using JobBoard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
	public class CompanyController : Controller
	{
		private DataContext _dataContext;
		private UserManager<AppUser> _userManager;
		private IWebHostEnvironment _env;

		public CompanyController(DataContext dataContext,UserManager<AppUser> userManager,IWebHostEnvironment env)
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

		[Authorize(Roles = "SuperAdmin,Member")]
		public async Task<IActionResult> CreateCompany()
		{
			AppUser user = null;

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(User.Identity.Name);
				if (user is null) return NotFound();
			}

			bool check = _dataContext.Companies.Any(x => x.AppUserId == user.Id && x.IsDeleted == false);
			CompanyViewModel companyVM = new CompanyViewModel
			{
				CheckCompany = check,
				CompanyId = _dataContext.Companies.FirstOrDefault(x => x.AppUserId == user.Id && x.IsDeleted == false)?.Id,
				IsBanned = _dataContext.Companies.FirstOrDefault(x => x.AppUserId == user.Id)?.IsBanned
			};

			return View(companyVM);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCompany(CompanyViewModel companyVM)
		{
			if (!ModelState.IsValid) return View(companyVM);
			if (companyVM.ImageFile is null)
			{
				ModelState.AddModelError("ImageFile", "Image File is required!");
				return View(companyVM);
			}
			AppUser user = null;

			Company newCompany = TypeConvert.Converter<CompanyViewModel, Company>(companyVM);

			if (User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(User.Identity.Name);
				if (user is null) return NotFound();

				newCompany.AppUserId = user.Id;
			}

			newCompany.IsActive = false;
			newCompany.IsView = false;
			newCompany.CreatedDate = DateTime.Now;

			if (companyVM.ImageFile.ContentType != "image/png" && companyVM.ImageFile.ContentType != "image/jpeg" && companyVM.ImageFile.ContentType != "image/jpg")
			{
				ModelState.AddModelError("ImageFile", "Yalniz Shekil fayli ola biler!");
				return View(companyVM);
			}
			if (companyVM.ImageFile.Length > 3145728)
			{
				ModelState.AddModelError("ImageFile", "Faylin ölçüsü max 3 mb ola biler!");
				return View(companyVM);
			}

			newCompany.LogoImage = FileManager.FileSave(_env.WebRootPath, "uploads\\companies", companyVM.ImageFile);
			_dataContext.Companies.Add(newCompany);
			_dataContext.SaveChanges();

			return RedirectToAction("Index", "Home");
		}


		[Authorize(Roles = "Company")]
		public IActionResult EditCompany(int id)
		{
			var existCompany = _dataContext.Companies.FirstOrDefault(x => x.Id == id);
			if (existCompany == null) return NotFound();

			CompanyViewModel companyVM = TypeConvert.Converter<Company, CompanyViewModel>(existCompany);
			companyVM.CompanyId = id;
			return View(companyVM);
		}

		[HttpPost]
		public IActionResult EditCompany(CompanyViewModel companyVM)
		{
			if (!ModelState.IsValid) return View(companyVM);

			var existCompany = _dataContext.Companies.FirstOrDefault(x => x.Id == companyVM.CompanyId);
			if (existCompany == null) return NotFound();

			//LostCompany lostCompany = TypeConvert.Converter<Company, LostCompany>(existCompany);

			existCompany = TypeConvert.Converter<CompanyViewModel, Company>(companyVM, existCompany);

			if (companyVM.ImageFile is not null)
			{
				if (companyVM.ImageFile.ContentType != "image/png" && companyVM.ImageFile.ContentType != "image/jpeg" && companyVM.ImageFile.ContentType != "image/jpg")
				{
					ModelState.AddModelError("ImageFile", "Yalniz Shekil fayli ola biler!");
					return View(companyVM);
				}
				if (companyVM.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "Faylin ölçüsü max 3 mb ola biler!");
					return View(companyVM);
				}

				FileManager.FileDelete(_env.WebRootPath, "uploads\\companies", existCompany.LogoImage);

				existCompany.LogoImage = FileManager.FileSave(_env.WebRootPath, "uploads\\companies", companyVM.ImageFile);
			}
			_dataContext.SaveChanges();

			return RedirectToAction("Index", "Home");
		}

		[Authorize(Roles = "Company")]
		public IActionResult CompanyDetail(string id)
		{
			var company = _dataContext.Companies.FirstOrDefault(x => x.AppUserId == id);
			if (company == null) return NotFound();

			CompanyViewModel companyVM = TypeConvert.Converter<Company, CompanyViewModel>(company);

			return View(companyVM);
		}
	}
}
