using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class Header
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:50)]
        public string Key { get; set; }
        [Required]
        [StringLength(maximumLength:100)]
        public string Value { get; set; }
    }
}
