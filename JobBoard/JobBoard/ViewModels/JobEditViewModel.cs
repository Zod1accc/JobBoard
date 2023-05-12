using JobBoard.Models;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.ViewModels
{
	public class JobEditViewModel
	{
		public int? Id { get; set; }
		public int JobTypeId { get; set; }
		public int GenderId { get; set; }
		[Required]
		[StringLength(maximumLength: 50)]
		public string JobTitle { get; set; }
		[Required]
		public string JobDescription { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		public string Location { get; set; }
		[Required]
		[StringLength(maximumLength: 50)]
		public string Experience { get; set; }
		[Required]
		[StringLength(maximumLength: 50)]
		public string Salary { get; set; }
		[Required]
		public int Vacancy { get; set; }
		public DateTime Deadline { get; set; }
		
		public IFormFile? ImageFile { get; set; }
    }
}
