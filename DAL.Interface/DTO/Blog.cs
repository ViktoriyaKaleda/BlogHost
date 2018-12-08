using System;
using System.Collections.Generic;

namespace DAL.Interface.DTO
{
	public class Blog
	{
		public int BlogId { get; set; }

		public string AuthorId { get; set; }

		public virtual ApplicationUser Author { get; set; }

		public string BlogName { get; set; }

		public string Description { get; set; }

		public DateTime CreatedDateTime { get; set; }

		public DateTime UpdatedDateTime { get; set; }

		public virtual List<Post> Posts { get; set; }

		public virtual List<BlogModerator> BlogModerators { get; set; }

		public virtual BlogStyle BlogStyle { get; set; }
		
		public string ImagePath { get; set; }

		public Blog()
		{
			Posts = new List<Post>();
			BlogModerators = new List<BlogModerator>();
		}
	}
}
