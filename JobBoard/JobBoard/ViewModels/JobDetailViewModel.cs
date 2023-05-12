using JobBoard.Models;

namespace JobBoard.ViewModels
{
	public class JobDetailViewModel
	{
        public JobDetail job { get; set; }
        public bool check { get; set; }
        public List<JobDetail> relatedJobs { get; set; }
    }
}
