using JobBoard.Models;

namespace JobBoard.ViewModels
{
	public class HomeViewModel
	{
        public List<JobDetail> jobs { get; set; }
		public List<AppUser> appUsers { get; set; }
        public int activeJobsCount { get; set; }
        public int deactiveJobsCount { get; set; }
        public int companyCount { get; set; }
    }
}
