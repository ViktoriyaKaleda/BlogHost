using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BlogH.Models
{
	public class Post
	{
		public int PostId { get; set; }

		public Blog Blog { get; set; }

		public IdentityUser Author { get; set; }

		public string Title { get; set; }

		public string Text { get; set; }

		public List<Tag> Tags { get; set; }

		public List<Like> Likes { get; set; }

		public List<Comment> Comments { get; set; }

		public DateTime CreatedDateTime { get; set; }

		public DateTime UpdatedDateTime { get; set; }

	}
}
