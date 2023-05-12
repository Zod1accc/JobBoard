using JobBoard.Areas.Manage.ViewModels;
using JobBoard.Models.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class StatusController : Controller
	{
		private DataContext _dataContext;

		public StatusController(DataContext dataContext)
        {
			_dataContext = dataContext;
		}
        public IActionResult Index()
		{
			
			StatusViewModel statusVM = new StatusViewModel
			{

				Title = _dataContext.Status
							.Where(x => x.Value == "Title").FirstOrDefault()?.Key,
				Desc = _dataContext.Status
							.Where(x => x.Value == "Desc").FirstOrDefault()?.Key
			};
			return View(statusVM);
		}

		[HttpPost]
		public IActionResult StatusUpdate(StatusViewModel statusVM)
		{
			if (statusVM.Title is null && statusVM.Desc is null)
			{
				//TempData["Error"] = ModelState.ValidationState.ToString();
				//TempData["error"] = ModelState.Values.SelectMany(v => v.Errors);

				//string messages = string.Join("; ", ModelState.Values
				//						.SelectMany(x => x.Errors)
				//						.Select(x => x.ErrorMessage));

				
				return RedirectToAction(nameof(Index));
			}

			if(statusVM.Title != null)
			{
				var existTitle = _dataContext.Status.FirstOrDefault(x => x.Value.ToLower() == "title");
				if (existTitle == null) return NotFound();

				existTitle.Key = statusVM.Title;
			}
			else if(statusVM.Desc != null)
			{
				var existDesc = _dataContext.Status.FirstOrDefault(x => x.Value.ToLower() == "desc");
				if(existDesc == null) return NotFound();

				existDesc.Key = statusVM.Desc;
			}

			_dataContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
	}
}
