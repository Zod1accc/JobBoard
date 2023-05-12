using System.ComponentModel.DataAnnotations;

namespace JobBoard.ViewModels.AccountViewModels
{
	public class RegisterViewModel
	{
        [Required]
        [StringLength(maximumLength:50)]
        public string Fullname { get; set; }
		[Required]
		[StringLength(maximumLength: 50)]
		public string Username { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
        [Required]
        [StringLength(maximumLength:50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
		[Required]
		[StringLength(maximumLength: 50)]
		[DataType(DataType.Password)]
		[Compare(nameof(Password))]
		public string ConfirmPassword { get; set; }
    }
}
