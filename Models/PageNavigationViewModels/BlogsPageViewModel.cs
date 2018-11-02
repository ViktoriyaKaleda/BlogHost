using BlogH.Models;
using System.Collections.Generic;

namespace BlogHosting.Models.PageNavigationViewModels
{
	public class BlogsPageViewModel
	{
		public IEnumerable<Blog> Blogs { get; set; }
		public PageViewModel PageViewModel { get; set; }
	}
}
