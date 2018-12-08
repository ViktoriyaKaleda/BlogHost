using System.ComponentModel.DataAnnotations;

namespace BlogHosting.Models.AccountViewModels
{
	public class LoginViewModel
	{
		[Required]
		[RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessage = "Use letters, numbers and symbol '-' only please.")]
		[Display(Name = "Username")]
		public string Username { get; set; }

		[Display(Name = "Password")]
		[Required(ErrorMessage = "Password field is required.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}
}
