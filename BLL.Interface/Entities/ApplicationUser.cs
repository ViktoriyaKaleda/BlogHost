using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BLL.Interface.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }
		
		public string AvatarPath { get; set; }

		public List<Blog> Blogs { get; set; }

		public List<Post> Posts { get; set; }

		public ApplicationUser() : base()
		{
			Blogs = new List<Blog>();
			Posts = new List<Post>();
		}
	}
}
