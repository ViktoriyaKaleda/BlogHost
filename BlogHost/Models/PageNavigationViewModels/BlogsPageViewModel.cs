using BLL.Interface.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BlogHost.Models.PageNavigationViewModels
{
	public class BlogsPageViewModel
	{
		public IEnumerable<Blog> Blogs { get; set; }

		public PageViewModel PageViewModel { get; set; }

		public List<SelectListItem> FiltersSelectListItems { get; set; }

		public string CurrentFilterNumber { get; set; }
	}
}
