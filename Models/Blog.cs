using BlogHosting.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogH.Models
{
	public class Blog
	{
		public int BlogId { get; set; }

		public virtual ApplicationUser Author { get; set; }

		public string BlogName { get; set; }

		public string Description { get; set; }

		public DateTime CreatedDateTime { get; set; }

		public DateTime UpdatedDateTime { get; set; }

		public virtual List<Post> Posts { get; set; }

		public virtual List<BlogModerator> BlogModerators { get; set; }

		public virtual BlogStyle BlogStyle { get; set; }

		[DataType(DataType.Upload)]
		public string ImagePath { get; set; }

		public Blog()
		{
			Posts = new List<Post>();
			BlogModerators = new List<BlogModerator>();
		}
	}
}
