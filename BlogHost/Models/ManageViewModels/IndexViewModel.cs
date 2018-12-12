using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogHost.Models.ManageViewModels
{
	public class IndexViewModel
	{
		public string Username { get; set; }

		public bool IsEmailConfirmed { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Last name")]
		public string LastName { get; set; }

		[DataType(DataType.Upload)]
		[Display(Name = "Avatar")]
		public IFormFile AvatarFile { get; set; }

		[Phone]
		[Display(Name = "Phone number")]
		public string PhoneNumber { get; set; }

		public string AvatarPath { get; set; }

		public string StatusMessage { get; set; }
	}
}
