using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogH.Models;
using BlogHosting.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using BlogHosting.Models.PostViewModels;
using BlogHosting.Models.BlogViewModels;
using BlogHosting.Models;
using Microsoft.AspNetCore.Authorization;
using BlogHosting.Models.PageNavigationViewModels;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using BlogHosting.Services;

namespace BlogHosting.Controllers
{
	public class BlogsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHostingEnvironment _appEnvironment;
		private readonly IAuthorizationService _authorizationService;
		private readonly ILogger _logger;
		private readonly IViewRenderService _viewRenderService;
		private readonly IImageService _imageService;

		public BlogsController(
				ApplicationDbContext context,
				UserManager<ApplicationUser> userManager,
				IHostingEnvironment appEnvironment,
				IAuthorizationService authorizationService,
				ILogger<BlogsController> logger,
				IViewRenderService viewRenderService,
				IImageService imageService
			)
		{
			_context = context;
			_userManager = userManager;
			_appEnvironment = appEnvironment;
			_authorizationService = authorizationService;
			_logger = logger;
			_viewRenderService = viewRenderService;
			_imageService = imageService;
		}

		// GET: Blogs
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

			IQueryable<Blog> source;
			switch (filterNumber)
			{
				case "1":   // recently created first
					source = _context.Blog.OrderByDescending(m => m.CreatedDateTime);
					break;
				case "2":   // first created first
					source = _context.Blog.OrderBy(m => m.CreatedDateTime);
					break;
				case "3":   // with more likes first
					source = _context.Blog.OrderByDescending(m => m.Posts.Sum(p => p.Likes.Count));
					break;
				case "4":   // with more posts first
					source = _context.Blog.OrderByDescending(m => m.Posts.Count);
					break;
				default:    // recently created first by default
					source = _context.Blog.OrderByDescending(m => m.CreatedDateTime);
					break;
			}

			var count = await source.CountAsync();
			var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

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
			{
				return NotFound();
			}

			var blog = await _context.Blog
				.SingleOrDefaultAsync(m => m.BlogId == id);

			if (blog == null)
			{
				return NotFound();
			}

			var posts = blog.Posts.Where(
					m => searchText == null
					|| m.Title.Contains(searchText)
					|| m.Text.Contains(searchText)
					|| m.Tags.Select(t => t.Name).Contains(searchText)
				).Select(m => new PostPreviewViewModel()
			{
				PostId = m.PostId,
				Title = m.Title,
				Text = m.Text,
				Author = m.Author,
				CreatedDateTime = m.CreatedDateTime,
				UpdatedDateTime = m.UpdatedDateTime,
				Tags = m.Tags,
				LikesNumber = _context.Like.Where(like => like.Post == m).Count(),
				CommentsNumber = _context.Comment.Where(c => c.Post == m).Count(),
				ImagePath = m.ImagePath,
				Blog = blog
			}).ToList();

			int pageSize = 3;

			var count = posts.Count();

			var items = posts.Skip((page - 1) * pageSize).Take(pageSize);

			PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

			BlogsDetailsPageViewModel viewModel = new BlogsDetailsPageViewModel
			{
				PageViewModel = pageViewModel,
				Blog = blog,
				Posts = items,
				CurrentSearchText = searchText
			};

			return View(viewModel);
		}

		[HttpPost("[Controller]/Edit/{id}/AddModerator")]
		public async Task<IActionResult> AddModerator(int id, [FromBody] string text)
		{
			if (String.IsNullOrEmpty(text))
				return BadRequest("Field can not be empty.");

			var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == text);

			if (user == null)
				return NotFound("User is not found.");

			if (user == await _userManager.GetUserAsync(HttpContext.User))
				return BadRequest("You already have absolute rights for this blog.");

			var blog = await _context.Blog.Include(m => m.BlogModerators).FirstOrDefaultAsync(m => m.BlogId == id);
			if (blog == null)
				return NotFound();

			if (blog.BlogModerators.FirstOrDefault(m => m.ModeratorId == user.Id) != null)
				return BadRequest("This user is already moderator.");

			blog.BlogModerators.Add(new BlogModerator() { Blog = blog, Moderator = user });
			_context.Update(blog);
			await _context.SaveChangesAsync();

			return PartialView("~/Views/Blogs/ModeratorsPartial.cshtml", blog.BlogModerators);
		}

		[HttpPost("[Controller]/Edit/{id}/DeleteModerator")]
		public async Task<IActionResult> DeleteModerator(int id, [FromBody] string text)
		{
			if (String.IsNullOrEmpty(text))
				return BadRequest("Field can not be empty.");

			var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == text);

			if (user == null)
				return NotFound("User is not found.");

			var blog = await _context.Blog.FirstOrDefaultAsync(m => m.BlogId == id);
			if (blog == null)
				return NotFound();

			var oldModerator = blog.BlogModerators.FirstOrDefault(m => m.ModeratorId == user.Id);

			_context.BlogModerator.Remove(oldModerator);

			await _context.SaveChangesAsync();

			return PartialView("~/Views/Blogs/ModeratorsPartial.cshtml", blog.BlogModerators);
		}

		// GET: Blogs/Create
		[Authorize]
		public async Task<IActionResult> Create()
		{
			var styles = await _context.BlogStyle.ToListAsync();
			var selectItems = new List<SelectListItem>();
			foreach (var style in styles)
			{
				selectItems.Add(new SelectListItem() { Text = style.BlogStyleName, Value = style.BlogStyleId.ToString() });
			}

			var viewModel = new BlogCreateViewModel()
			{
				Styles = selectItems,
			};
			return View(viewModel);
		}

		// POST: Blogs/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
					Author = await _userManager.GetUserAsync(HttpContext.User),
					CreatedDateTime = DateTime.Now,
					UpdatedDateTime = DateTime.Now,
				};

				var style = await _context.BlogStyle.SingleOrDefaultAsync(m => m.BlogStyleId == viewModel.BlogStyleId);

				blog.BlogStyle = style;

				blog.ImagePath = style.DefaultImagePath;

				if (viewModel.ImageFile?.FileName != null)
				{
					blog.ImagePath = await _imageService.SaveBlogImage(viewModel.ImageFile);
				}

				_context.Add(blog);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(viewModel);
		}

		[HttpPost("[Controller]/Edit/{id}/SetStyle")]
		public async Task<IActionResult> SetStyle([FromBody] string styleId)
		{
			var style = await _context.BlogStyle.SingleOrDefaultAsync(m => m.BlogStyleId == Convert.ToInt32(styleId));
			if (style == null)
				return BadRequest();

			return new ObjectResult(
				new
				{
					style.BackgrounsColor,
					style.TitlesFontColor,
					style.TitlesFontName,
					style.SecondColor,
					style.DefaultImagePath
				});
		}

		// GET: Blogs/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var blog = await _context.Blog.FirstOrDefaultAsync(m => m.BlogId == id);
			if (blog == null)
			{
				return NotFound();
			}

			var isOwner = await _authorizationService.AuthorizeAsync(User, blog, "OwnerPolicy");
			if (isOwner.Succeeded)
			{
				var styles = await _context.BlogStyle.ToListAsync();
				var selectItems = new List<SelectListItem>();
				foreach (var style in styles)
				{
					selectItems.Add(new SelectListItem() { Text = style.BlogStyleName, Value = style.BlogStyleId.ToString() });
				}

				var viewModel = new BlogEditViewModel()
				{
					BlogId = (int)id,
					BlogName = blog.BlogName,
					Description = blog.Description,
					ImagePath = blog.ImagePath,
					Moderators = blog.BlogModerators,
					Styles = selectItems,
					CurrentStyle = blog.BlogStyle,
					BlogStyleId = blog.BlogStyle.BlogStyleId
				};

				return View(viewModel);
			}

			else
			{
				return Forbid();
			}

		}

		// POST: Blogs/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit(int id, BlogEditViewModel viewModel)
		{
			if (id != viewModel.BlogId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{

				var blog = _context.Blog.FirstOrDefault(m => m.BlogId == id);
				if (blog == null)
					return NotFound();

				blog.BlogName = viewModel.BlogName;
				blog.Description = viewModel.Description;
				blog.UpdatedDateTime = DateTime.Now;

				var style = await _context.BlogStyle.SingleOrDefaultAsync(m => m.BlogStyleId == viewModel.BlogStyleId);

				blog.BlogStyle = style;

				blog.ImagePath = style.DefaultImagePath;

				if (viewModel.ImageFile?.FileName != null)
				{
					blog.ImagePath = await _imageService.SaveBlogImage(viewModel.ImageFile, blog.ImagePath);
				}

				_context.Update(blog);
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(Index));
			}
			return View(viewModel);
		}

		// GET: Blogs/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var blog = await _context.Blog.FirstOrDefaultAsync(m => m.BlogId == id);
			if (blog == null)
			{
				return NotFound();
			}

			var isOwner = await _authorizationService.AuthorizeAsync(User, blog, "OwnerPolicy");
			if (isOwner.Succeeded)
			{
				return View(blog);
			}

			else
			{
				return Forbid();
			}
		}

		// POST: Blogs/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var blog = await _context.Blog.FindAsync(id);

			if (blog.BlogModerators.Count != 0)
			{
				foreach (var blogModerator in blog.BlogModerators)
				{
					_context.BlogModerator.Remove(blogModerator);
				}
				await _context.SaveChangesAsync();
			}

			if (blog.ImagePath != null)
			{
				_imageService.DeleteBlogImage(blog.ImagePath);
			}

			_context.Blog.Remove(blog);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool BlogExists(int id)
		{
			return _context.Blog.Any(e => e.BlogId == id);
		}
	}
}
