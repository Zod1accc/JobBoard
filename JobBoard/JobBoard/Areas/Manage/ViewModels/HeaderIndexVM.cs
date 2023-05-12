using JobBoard.Models;

namespace JobBoard.Areas.Manage.ViewModels
{
	public class HeaderIndexVM
	{
		public List<Header>? headerItems { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? Title { get; set; }
        public string? Desc { get; set; }
    }
}
