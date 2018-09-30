using BlogH.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BlogHosting.Models.PostViewModels
{
	public class PostPreviewViewModel
	{
		public int PostId { get; set; }

		public string Title { get; set; }

		public string Text { get; set; }

		public IdentityUser Author { get; set; }

		public DateTime CreatedDateTime { get; set; }

		public DateTime UpdatedDateTime { get; set; }

		public List<Tag> Tags { get; set; }

		public int LikesNumber { get; set; }

		public int CommentsNumber { get; set; }
	}
}
