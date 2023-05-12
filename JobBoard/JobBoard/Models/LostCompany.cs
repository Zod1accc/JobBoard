using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
	public class LostCompany
	{
		public int Id { get; set; }
		public int CompanyId { get; set; }
		public string AppUserId { get; set; }
		[Required]
		[StringLength(maximumLength: 80)]
		public string Name { get; set; }
		[Required]
		public string Desc { get; set; }
		[Required]
		[StringLength(maximumLength: 100)]
		public string Tagline { get; set; }
		public string WebsiteLink { get; set; }
		public string Facebook { get; set; }
		public string Linkedin { get; set; }
		public string Twitter { get; set; }
		public string LogoImage { get; set; }
		public bool IsView { get; set; }
		public bool IsUpdated { get; set; }
        public Company Company { get; set; }

    }
}
