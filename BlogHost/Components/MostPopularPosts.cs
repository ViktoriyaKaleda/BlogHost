using BLL.Interface.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHosting.Components
{
	public class MostPopularPosts : ViewComponent
	{
		private readonly IPostService _postService;
		private readonly IAccountService _accountService;

		public MostPopularPosts(IPostService postService, IAccountService accountService)
		{
			_postService = postService;
			_accountService = accountService;
		}

		public async Task<IViewComponentResult> InvokeAsync(string authorId, int postId)
		{
			var author = await _accountService.GetUserById(authorId);
			var posts = _postService.GetAllPosts()
				.Where(m => m.Author.Id == authorId && m.PostId != postId )
				.OrderByDescending(m => m.Likes.Count)
				.Take(5).ToList();

			ViewData["Posts"] = posts;
			ViewData["Author"] = author;
			return View();
		}
	}
}
