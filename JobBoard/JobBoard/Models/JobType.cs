using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
	public class JobType
	{
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }
        public List<JobDetail> JobDetails { get; set; }
    }
}
