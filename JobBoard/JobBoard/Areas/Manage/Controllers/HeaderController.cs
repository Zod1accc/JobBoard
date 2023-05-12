using JobBoard.Areas.Manage.ViewModels;
using JobBoard.Helpers;
using JobBoard.Models.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class HeaderController : Controller
    {
        private DataContext _dataContext;
		private IWebHostEnvironment _env;

		public HeaderController(DataContext dataContext,IWebHostEnvironment env)
        {
            _dataContext = dataContext;
            _env = env;
        }
        public IActionResult Index()
        {
            ViewBag.Image = _dataContext.Header.FirstOrDefault(x => x.Key == "Image").Value;
            HeaderIndexVM headerIndexVM = new HeaderIndexVM
            {
				headerItems = _dataContext.Header.Where(x => x.Key != "Image").ToList(),
                Image = _dataContext.Header.FirstOrDefault(x=>x.Key == "Image").Value
		    };
            return View(headerIndexVM);
        }

        public IActionResult HeaderImage(HeaderIndexVM headerIndexVM)
        {
            var existHeaderImage = _dataContext.Header.FirstOrDefault(x => x.Key == "Image");
            if (existHeaderImage == null) return NotFound();
            
            if(headerIndexVM.ImageFile != null)
            {
                if(headerIndexVM.ImageFile.ContentType != "image/jpeg" && headerIndexVM.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Just image file!");
                    return RedirectToAction(nameof(Index));
                }

                if(headerIndexVM.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "Max 3 mb!");
					return RedirectToAction(nameof(Index));
				}
                FileManager.FileDelete(_env.WebRootPath, "uploads\\header", existHeaderImage.Value);
                existHeaderImage.Value = FileManager.FileSave(_env.WebRootPath, "uploads\\header", headerIndexVM.ImageFile);
                _dataContext.SaveChanges();
			}
            return RedirectToAction(nameof(Index));
        }

        public IActionResult HeaderContext(HeaderIndexVM headerVM)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            if(headerVM.Title is not null)
            {
                var existTitle = _dataContext.Header
                        .Where(x => x.Key.ToLower() == "title").FirstOrDefault();
                if (existTitle is null) return NotFound();

                existTitle.Value = headerVM.Title;
            }
            else if(headerVM.Desc is not null)
            {
                var existDesc = _dataContext.Header
                        .Where(x => x.Key.ToLower() == "desc").FirstOrDefault();
                if (existDesc is null) return NotFound();

                existDesc.Value = headerVM.Desc;
            }

            _dataContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
