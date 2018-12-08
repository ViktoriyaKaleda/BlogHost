using System;
using System.Collections.Generic;

namespace DAL.Interface.DTO
{
	public class Post
	{
		public int PostId { get; set; }

		public int BlogId { get; set; }

		public virtual Blog Blog { get; set; }

		public string AuthorId { get; set; }

		public virtual ApplicationUser Author { get; set; }

		public string Title { get; set; }

		public string Text { get; set; }

		public virtual List<Tag> Tags { get; set; }

		public virtual List<Like> Likes { get; set; }

		public virtual List<Comment> Comments { get; set; }

		public DateTime CreatedDateTime { get; set; }

		public DateTime UpdatedDateTime { get; set; }
		
		public string ImagePath { get; set; }
	}
}
