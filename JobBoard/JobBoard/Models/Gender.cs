using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
	public class Gender
	{
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:20)]
        public string GenderName { get; set; }
    }
}
