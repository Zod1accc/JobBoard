using JobBoard.Models;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Areas.Manage.ViewModels
{
	public class StatusViewModel
	{

		[StringLength(maximumLength: 80)]
		public string? Title { get; set; }
	
		[StringLength(maximumLength: 100)]
		public string? Desc { get; set; }
    }
}
