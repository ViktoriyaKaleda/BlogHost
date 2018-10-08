using BlogHosting.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHosting.Components
{
	public class MostPopularPosts : ViewComponent
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public MostPopularPosts(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public IViewComponentResult Invoke(string authorId, int postId)
		{
			var author = _context.Users.SingleOrDefault(m => m.Id == authorId);
			var posts = _context.Post
				.Where(m => m.Author == author && m.PostId != postId )
				.Include(m => m.Likes)
				.Include(m => m.Comments)
				.OrderByDescending(m => m.Likes.Count)
				.Take(5).ToList();

			ViewData["Posts"] = posts;
			ViewData["Author"] = author;
			return View();
		}
	}
}
