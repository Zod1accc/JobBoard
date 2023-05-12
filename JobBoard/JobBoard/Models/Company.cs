using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoard.Models
{
	public class Company
	{
		public int Id { get; set; }
        public string AppUserId { get; set; }
        [Required]
        [StringLength(maximumLength:80)]
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
        public bool IsActive { get; set; }
        public bool IsView { get; set; }
        public bool IsUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBanned { get; set; }
        public DateTime CreatedDate { get; set; }
        public AppUser AppUser { get; set; }
        public List<JobDetail> Jobs { get; set; }
        //public List<LostCompany> LostCompanies { get; set; }

    }
}
