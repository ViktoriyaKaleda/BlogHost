using BlogH.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogHosting.Models.PostViewModels
{
	public class PostEditViewModel
	{
		public int PostId { get; set; }

		public string Title { get; set; }

		public string Text { get; set; }

		public string ImagePath { get; set; }

		public string[] StringTags { get; set; }

		[DataType(DataType.Upload)]
		public IFormFile ImageFile { get; set; }

		public Blog Blog { get; set; }

		public PostEditViewModel() { }

		public PostEditViewModel(int postId, string title, string text, List<Tag> tags, string imagePath, Blog blog)
		{
			PostId = postId;
			Title = title;
			Text = text;
			ImagePath = imagePath;
			Blog = blog;
			StringTags = new string[tags.Count];
			for (int i = 0; i < tags.Count; i++)
				StringTags[i] = tags[i].Name;
		}
	}
}
