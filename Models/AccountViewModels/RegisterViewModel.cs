using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHosting.Models.AccountViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "The Username field is required.")]
		[RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessage = "Use letters, numbers and symbol '-' only please.")]
		[Display(Name = "Username")]
		public string Username { get; set; }

		[Required(ErrorMessage = "The First name field is required.")]
		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "The Second name field is required.")]
		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "The Email field is required.")]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "The Password field is required.")]
		[StringLength(10, ErrorMessage = "The password must be at least 6 and at max 10 characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[DataType(DataType.Upload)]
		[Display(Name = "Avatar")]
		public IFormFile AvatarFile { get; set; }
	}
}
