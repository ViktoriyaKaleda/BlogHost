using System;
using System.Collections.Generic;

namespace BLL.Interface.Entities
{
	public class Blog
	{
		public int BlogId { get; set; }

		public string AuthorId { get; set; }

		public string BlogName { get; set; }

		public string Description { get; set; }

		public DateTime CreatedDateTime { get; set; }

		public DateTime UpdatedDateTime { get; set; }

		public List<Post> Posts { get; set; }

		public BlogStyle BlogStyle { get; set; }
		
		public string ImagePath { get; set; }

		public Blog()
		{
			Posts = new List<Post>();
		}
	}
}
