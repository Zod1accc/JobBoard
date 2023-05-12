using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
	public class JobDetail
	{
        public int Id { get; set; }
        public int JobTypeId { get; set; }
        public int CompanyId { get; set; }
        public int GenderId { get; set; }
        [Required]
        [StringLength(maximumLength:50)]
        public string JobTitle { get; set; }
        [Required]
        public string JobDescription { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Location { get; set; }
        public string Image { get; set; }
        [Required]
        [StringLength (maximumLength:50)]
        public string Experience { get; set; }
		[Required]
		[StringLength(maximumLength: 50)]
		public string Salary { get; set; }
        [Required]
		public int Vacancy { get; set; }
		public DateTime PublishedOn { get; set; }
		public DateTime Deadline { get; set; }
        public bool IsActive { get { return DateTime.Now < Deadline; } }
        public JobType JobType { get; set; }
        public Company Company { get; set; }
        public Gender Gender { get; set; }
    }
}
