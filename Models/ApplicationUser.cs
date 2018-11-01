using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlogHosting.Models
{
	public class ApplicationUser : IdentityUser
	{
		public ApplicationUser() : base() { }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		[DataType(DataType.Upload)]
		public string AvatarPath { get; set; }
	}
}
