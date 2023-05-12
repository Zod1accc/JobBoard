using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
	public class Status
	{
        public int Id { get; set; }
        public string Key { get; set; }
        [Required]
        [StringLength(maximumLength:100)]
        public string Value { get; set; }
    }
}
