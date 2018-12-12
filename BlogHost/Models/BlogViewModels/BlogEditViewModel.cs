using BLL.Interface.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogHost.Models.BlogViewModels
{
	public class BlogEditViewModel
	{
		public int BlogId { get; set; }

		[Display(Name = "Name")]
		public string BlogName { get; set; }

		public string Description { get; set; }

		public List<ApplicationUser> Moderators { get; set; }

		public string ImagePath { get; set; }

		[Display(Name = "Style")]
		public int BlogStyleId { get; set; }

		public List<SelectListItem> Styles { get; set; }

		public BlogStyle CurrentStyle { get; set; }

		[DataType(DataType.Upload)]
		[Display(Name = "Header image")]
		public IFormFile ImageFile { get; set; }
	}
}
