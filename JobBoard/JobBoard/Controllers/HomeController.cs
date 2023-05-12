using JobBoard.Models;
using JobBoard.Models.DataContext;
using JobBoard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Controllers
{
    public class HomeController : Controller
    {
		private DataContext _dataContext;

		public HomeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
			ViewBag.JobTypes = _dataContext.JobTypes.ToList();

			// select members
			string roleId = _dataContext.Roles.FirstOrDefault(x => x.Name.ToLower() == "member").Id;
            if(roleId == null) return NotFound();

            var selectUsers = _dataContext.UserRoles
                                     .Where(x => x.RoleId == roleId).ToList();
            List<AppUser> users = null;

            if(selectUsers.Count > 0)
            {
                users = _dataContext.Users
                                .AsEnumerable()
                                .Where(x => selectUsers.All(s => x.Id == s.UserId)).ToList();
            }

            //home VM
            HomeViewModel homeVM = new HomeViewModel
            {
                jobs = _dataContext.Jobs
                                .Include(x => x.JobType)
                                .Include(x => x.Gender)
                                .Include(x => x.Company).ToList(),
                appUsers = users,

                companyCount = _dataContext.Companies.Count(),
                
                activeJobsCount = _dataContext.Jobs
                                          .AsEnumerable()
                                          .Where(x=>x.IsActive == true).Count(),
                
                deactiveJobsCount = _dataContext.Jobs
                                          .AsEnumerable()
										  .Where(x => x.IsActive == false).Count()

			};

            return View(homeVM);
        }

    }
}