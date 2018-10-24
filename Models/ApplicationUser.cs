using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
