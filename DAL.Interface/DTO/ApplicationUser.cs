using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DAL.Interface.DTO
{
	public class ApplicationUser : IdentityUser
	{
		public ApplicationUser() : base() { }

		public string FirstName { get; set; }

		public string LastName { get; set; }
		
		public string AvatarPath { get; set; }

		public virtual List<Blog> Blogs { get; set; }

		public virtual List<Post> Posts { get; set; }

		public virtual List<BlogModerator> BlogModerator { get; set; }
	}
}
