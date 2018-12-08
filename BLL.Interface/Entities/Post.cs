using System;
using System.Collections.Generic;

namespace BLL.Interface.Entities
{
	public class Post
	{
		public int PostId { get; set; }

		public int BlogId { get; set; }

		public string AuthorId { get; set; }

		public string Title { get; set; }

		public string Text { get; set; }

		public List<Tag> Tags { get; set; }

		public List<Like> Likes { get; set; }

		public List<Comment> Comments { get; set; }

		public DateTime CreatedDateTime { get; set; }

		public DateTime UpdatedDateTime { get; set; }
		
		public string ImagePath { get; set; }

		public Post()
		{
			Tags = new List<Tag>();
			Likes = new List<Like>();
			Comments = new List<Comment>();
		}
	}
}
