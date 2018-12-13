using BLL.Interface.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogHost.Models.PostViewModels
{
	public class PostCreateViewModel
	{
		public int PostId { get; set; }

		public string Title { get; set; }

		public string Text { get; set; }

		public string[] StringTags { get; set; }

		public string ImagePath { get; set; }

		[DataType(DataType.Upload)]
		public IFormFile ImageFile { get; set; }

		public Blog Blog { get; set; }
	}
}
