using BlogH.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
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

		public virtual List<Blog> Blogs { get; set; }

		public virtual List<Post> Posts { get; set; }

		public virtual List<BlogModerator> BlogModerator { get; set; }
	}
}
