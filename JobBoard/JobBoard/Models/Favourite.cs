namespace JobBoard.Models
{
	public class Favourite
	{
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public int JobDetailId { get; set; }
        public JobDetail JobDetail { get; set; }
        public AppUser AppUser { get; set; }
    }
}
