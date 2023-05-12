using JobBoard.Areas.Manage.ViewModels.CompanyViewModels;
using JobBoard.Models;
using JobBoard.Models.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.Manage.Controllers
{
	[Area(nameof(Manage))]
	public class CompanyController : Controller
	{
		private DataContext _dataContext;
		private UserManager<AppUser> _userManager;
		private SignInManager<AppUser> _signInManger;
		
		public CompanyController(DataContext dataContext,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _dataContext = dataContext;
			_userManager = userManager;
			_signInManger = signInManager;
        }
        public IActionResult Index(string? search)
		{

			IndexViewModel indexVM = new IndexViewModel();

			ViewBag.Search = search;

			var query = _dataContext.Companies.AsQueryable();

			if (search is not null)
			{
				string[] keywords = search.ToLower().Split(' ');

				var result = _dataContext.Companies
								.AsEnumerable()
								.Where(c=> keywords.All(k=>c.Name.ToLower().Contains(k))).ToList();

				indexVM.companies = result;
			}
			else
			{
				indexVM.companies = query.ToList();
			}

			return View(indexVM);
		}

		public async Task<IActionResult> CompanyDetail(int id)
		{
			AppUser user = null;

			var company = _dataContext.Companies.FirstOrDefault(c => c.Id == id);
			if (company is null) return NotFound();

			company.IsView = true;
			_dataContext.SaveChanges();

			user = _dataContext.Users.FirstOrDefault(x => x.Id == company.AppUserId);

			CompanyDetailviewModel model = new CompanyDetailviewModel
			{
				Company = company,
				User = user,
			};

			return View(model);
		}

		public async Task<IActionResult> CompanyActivate(int id)
		{
			var company = _dataContext.Companies.FirstOrDefault(c => c.Id == id);
			if(company is null) return NotFound();

			var user = _dataContext.Users.FirstOrDefault(x=>x.Id == company.AppUserId);
			if(user == null) return NotFound();

			var roleMember = _dataContext.UserRoles.FirstOrDefault(x => x.UserId == user.Id);
			if(roleMember is null) return NotFound();
			_dataContext.UserRoles.Remove(roleMember);

			await _userManager.AddToRoleAsync(user, "Company");

			company.IsActive = true;
			_dataContext.SaveChanges();

			return Ok();
		}

		public async Task<IActionResult> CompanyDeactivate(int id)
		{
			var company = _dataContext.Companies.FirstOrDefault(c => c.Id == id);
			if (company is null) return NotFound();

			AppUser user = await _userManager.FindByIdAsync(company.AppUserId);
			if (user is null) return NotFound();

			var roleMember = _dataContext.UserRoles.FirstOrDefault(x => x.UserId == user.Id);
			if(roleMember is null) return NotFound();

			_dataContext.UserRoles.Remove(roleMember);

			await _userManager.AddToRoleAsync(user, "Member");

			company.IsActive = false;
			company.IsBanned = true;
			_dataContext.SaveChanges();

			return Ok();
		}
	}
}