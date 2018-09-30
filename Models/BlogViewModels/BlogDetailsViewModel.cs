using BlogH.Models;
using BlogHosting.Models.PostViewModels;
using System.Collections.Generic;

namespace BlogHosting.Models.BlogViewModels
{
	public class BlogDetailsViewModel
	{
		public Blog Blog { get; set; }

		public List<PostPreviewViewModel> Posts { get; set; }
	}
}
