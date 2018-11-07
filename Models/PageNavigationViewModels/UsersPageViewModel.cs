using System.Collections.Generic;

namespace BlogHosting.Models.PageNavigationViewModels
{
	public class UsersPageViewModel
	{
		public IEnumerable<ApplicationUser> Users { get; set; }
		public PageViewModel PageViewModel { get; set; }
	}
}
