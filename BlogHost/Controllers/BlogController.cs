using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using BlogHost.Models.BlogViewModels;
using BlogHost.Models.PageNavigationViewModels;
using BlogHost.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogHost.Controllers
{
	public class BlogController : Controller
    {
		private readonly IBlogService _blogService;
		private readonly IAccountService _accountService;
		private readonly IImageService _imageService;
		private readonly IAuthorizationService _authorizationService;

		public BlogController(
			IBlogService blogService,
			IAccountService accountService,
			IImageService imageService,
			IAuthorizationService authorizationService)
		{
			_blogService = blogService;
			_accountService = accountService;
			_imageService = imageService;
			_authorizationService = authorizationService;
		}

		public async Task<IActionResult> Index(int page = 1, string filterNumber = null)
        {
			int pageSize = 3;   // number of blogs on page

			var filterVariants = new List<SelectListItem>
			{
				new SelectListItem { Text = "Recently created", Value="1" },
				new SelectListItem { Text = "First created", Value="2" },
				new SelectListItem { Text="Popularity" , Value="3" },
				new SelectListItem { Text="Number of posts" , Value="4" }
			};

			IEnumerable<Blog> source = _blogService.GetAllBlogs();
			switch (filterNumber)
			{
				case "1":   // recently created first
					source = source.OrderByDescending(m => m.CreatedDateTime);
					break;
				case "2":   // first created first
					source = source.OrderBy(m => m.CreatedDateTime);
					break;
				case "3":   // with more likes first
					source = source.OrderByDescending(m => m.Posts.Sum(p => p.Likes.Count));
					break;
				case "4":   // with more posts first
					source = source.OrderByDescending(m => m.Posts.Count);
					break;
				default:    // recently created first by default
					source = source.OrderByDescending(m => m.CreatedDateTime);
					break;
			}

			var count = source.ToList().Count;
			var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

			PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
			BlogsPageViewModel viewModel = new BlogsPageViewModel
			{
				PageViewModel = pageViewModel,
				Blogs = items,
				FiltersSelectListItems = filterVariants,
				CurrentFilterNumber = string.IsNullOrEmpty(filterNumber) ? "1" : filterNumber,
			};

			return View(viewModel);
		}

		// GET: Blogs/Details/5
		public async Task<IActionResult> Details(int? id, int page = 1, string searchText = null)
		{
			if (id == null)
				return NotFound();

			var blog = await _blogService.GetBlogById((int)id);
			if (blog == null)
				return NotFound();

			var posts = blog.Posts;

			int pageSize = 3;

			var count = posts.Count();

			var items = posts.Skip((page - 1) * pageSize).Take(pageSize);

			PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

			BlogDetailsPageViewModel viewModel = new BlogDetailsPageViewModel
			{
				PageViewModel = pageViewModel,
				Blog = blog,
				Posts = items,
				CurrentSearchText = searchText
			};

			return View(viewModel);
		}

		// GET: Blogs/Create
		[Authorize]
		public async Task<IActionResult> Create()
		{
			var styles = await _blogService.GetAllBlogStyles();

			var selectItems = styles.Select(m => 
				new SelectListItem { Text = m.BlogStyleName, Value = m.BlogStyleId.ToString() }).ToList();

			var viewModel = new BlogCreateViewModel { Styles = selectItems };

			return View(viewModel);
		}

		// POST: Blogs/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Create(BlogCreateViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var blog = new Blog()
				{
					BlogName = viewModel.BlogName,
					Description = viewModel.Description,
					Author = await _accountService.GetCurrentUser(User),
					CreatedDateTime = DateTime.Now,
					UpdatedDateTime = DateTime.Now,
				};				

				if (viewModel.ImageFile?.FileName != null)
					blog.ImagePath = await _imageService.SaveBlogImage(viewModel.ImageFile);

				await _blogService.AddBlog(blog, viewModel.BlogStyleId);
				return RedirectToAction(nameof(Index));
			}
			return View(viewModel);
		}

		// GET: Blogs/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
				return NotFound();

			var blog = await _blogService.GetBlogById((int)id);
			if (blog == null)
				return NotFound();

			var isOwner = await _authorizationService.AuthorizeAsync(User, blog, "OwnerPolicy");
			if (!isOwner.Succeeded)
				return Forbid();

			var styles = await _blogService.GetAllBlogStyles();
			var selectItems = styles.Select(m =>
			new SelectListItem { Text = m.BlogStyleName, Value = m.BlogStyleId.ToString() }).ToList();

			var viewModel = new BlogEditViewModel()
			{
				BlogId = (int)id,
				BlogName = blog.BlogName,
				Description = blog.Description,
				ImagePath = blog.ImagePath,
				Moderators = await _blogService.GetAllBlogModerators(blog.BlogId),
				Styles = selectItems,
				CurrentStyle = blog.BlogStyle,
				BlogStyleId = blog.BlogStyle.BlogStyleId
			};

			return View(viewModel);
		}

		// POST: Blogs/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit(int id, BlogEditViewModel viewModel)
		{
			if (id != viewModel.BlogId)
				return NotFound();

			if (ModelState.IsValid)
			{
				var blog = await _blogService.GetBlogById(id);
				if (blog == null)
					return NotFound();

				string imagePath = null;

				if (viewModel.ImageFile?.FileName != null)
					imagePath = await _imageService.SaveBlogImage(viewModel.ImageFile, blog.ImagePath);

				await _blogService.UpdateBlog(blog.BlogId, viewModel.BlogName, viewModel.Description, viewModel.BlogStyleId, imagePath);

				return RedirectToAction(nameof(Index));
			}
			return View(viewModel);
		}

		// GET: Blogs/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			var blog = await _blogService.GetBlogById((int)id);
			if (blog == null)
				return NotFound();

			var isOwner = await _authorizationService.AuthorizeAsync(User, blog, "OwnerPolicy");
			if (isOwner.Succeeded)
				return View(blog);
			else
				return Forbid();
		}

		// POST: Blogs/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var blog = await _blogService.GetBlogById(id);

			if (blog.ImagePath != null)
				_imageService.DeleteBlogImage(blog.ImagePath);

			await _blogService.DeleteBlog(id);

			return RedirectToAction(nameof(Index));
		}
	}
}