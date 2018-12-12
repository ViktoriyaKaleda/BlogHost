using BLL.Interface.Entities;
using System.Collections.Generic;

namespace BlogHost.Models.PageNavigationViewModels
{
	public class BlogDetailsPageViewModel
	{
		public Blog Blog { get; set; }

		public IEnumerable<Post> Posts { get; set; }

		public PageViewModel PageViewModel { get; set; }

		public string CurrentSearchText { get; set; }
	}
}
