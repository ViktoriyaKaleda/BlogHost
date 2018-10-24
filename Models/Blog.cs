using BlogHosting.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogH.Models
{
	public class Blog
	{
		public int BlogId { get; set; }

		public ApplicationUser Author { get; set; }

		public string BlogName { get; set; }

		public string Description { get; set; }

		public DateTime CreatedDateTime { get; set; }

		public DateTime UpdatedDateTime { get; set; }

		public List<Post> Posts { get; set; }

		//TODO
		//Rating, followers, styles, moderator
	}
}
