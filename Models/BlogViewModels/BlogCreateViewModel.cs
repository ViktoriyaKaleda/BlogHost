using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogHosting.Models.BlogViewModels
{
	public class BlogCreateViewModel
	{
		public int BlogId { get; set; }

		public string BlogName { get; set; }

		public string Description { get; set; }

		public string ImagePath { get; set; }

		public int BlogStyleId { get; set; }

		public List<SelectListItem> Styles { get; set; }

		[DataType(DataType.Upload)]
		public IFormFile ImageFile { get; set; }
	}
}
