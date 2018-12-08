using BlogH.Models;
using BlogHosting.Models.BlogViewModels;
using BlogHosting.Models.PostViewModels;
using System.Collections.Generic;

namespace BlogHosting.Models.PageNavigationViewModels
{
	public class BlogsDetailsPageViewModel
	{
		public Blog Blog { get; set; }

		public IEnumerable<PostPreviewViewModel> Posts { get; set; }

		public PageViewModel PageViewModel { get; set; }

		public string CurrentSearchText { get; set; }
	}
}
