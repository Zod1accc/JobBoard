namespace JobBoard.Models
{
	public class Blog
	{
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public Comment Comment { get; set; }

    }
}
