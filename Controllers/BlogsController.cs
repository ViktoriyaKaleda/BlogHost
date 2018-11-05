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

namespace BlogHosting.Controllers
{
	public class BlogsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHostingEnvironment _appEnvironment;
		private readonly IAuthorizationService _authorizationService;
		private readonly ILogger _logger;

		public BlogsController(
				ApplicationDbContext context,
				UserManager<ApplicationUser> userManager,
				IHostingEnvironment appEnvironment,
				IAuthorizationService authorizationService,
				ILogger<BlogsController> logger
			)
		{
			_context = context;
			_userManager = userManager;
			_appEnvironment = appEnvironment;
			_authorizationService = authorizationService;
			_logger = logger;
		}

		// GET: Blogs
		public async Task<IActionResult> Index(int page = 1)
		{
			int pageSize = 3;   // number of blogs on page

			IQueryable<Blog> source = _context.Blog.Include(m => m.Author).OrderByDescending(m => m.CreatedDateTime);
			var count = await source.CountAsync();
			var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
			BlogsPageViewModel viewModel = new BlogsPageViewModel
			{
				PageViewModel = pageViewModel,
				Blogs = items
			};

			return View(viewModel);
		}

		// GET: Blogs/Details/5
		public async Task<IActionResult> Details(int? id, int page = 1)
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

			var posts = blog.Posts.Select(m => new PostPreviewViewModel()
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
				Posts = items
			};

			return View(viewModel);
		}

		[HttpPost("[Controller]/Details/{id}/Search")]
		public async Task<IActionResult> Search([FromBody] string text)
		{
			var posts = await _context.Post
				.OrderByDescending(m => m.CreatedDateTime)
				.Where(
					m => m.Title.Contains(text)
					|| m.Text.Contains(text)
					|| m.Tags.Select(t => t.Name).Contains(text)
					|| text == null
				)
				.Select(m => new PostPreviewViewModel()
				{
					PostId = m.PostId,
					Title = m.Title,
					Text = m.Text,
					Author = m.Author,
					CreatedDateTime = m.CreatedDateTime,
					UpdatedDateTime = m.UpdatedDateTime,
					Tags = m.Tags,
					LikesNumber = _context.Like.Where(like => like.Post == m).Count(),
					CommentsNumber = _context.Comment.Where(c => c.Post == m).Count()
				})
				.ToListAsync();

			return PartialView("~/Views/Posts/PostPartial.cshtml", posts);
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
		public IActionResult Create()
		{
			return View();
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

				if (viewModel.ImageFile?.FileName != null)
				{
					string path = GetImagePath(viewModel.ImageFile);

					blog.ImagePath = "~/" + path;

					using (var fileStream = new FileStream(_appEnvironment.WebRootPath + "/" + path, FileMode.Create))
					{
						await viewModel.ImageFile.CopyToAsync(fileStream);
					}
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

			return new ObjectResult(style);
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
					CurrentStyle = await _context.BlogStyle.SingleOrDefaultAsync(m => m.BlogStyleName == "Default")
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

				if (viewModel.ImageFile?.FileName != null)
				{
					if (blog.ImagePath != null)
					{
						try
						{
							System.IO.File.Delete(_appEnvironment.WebRootPath + "/BlogImages/" + Path.GetFileName(blog.ImagePath));
						}
						catch (System.IO.IOException e)
						{
							_logger.LogWarning("Failed to delete blog image file. File path: {}", blog.ImagePath);
						}
					}

					string path = GetImagePath(viewModel.ImageFile);

					blog.ImagePath = "~/" + path;

					using (var fileStream = new FileStream(_appEnvironment.WebRootPath + "/" + path, FileMode.Create))
					{
						await viewModel.ImageFile.CopyToAsync(fileStream);
					}
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
				try
				{
					System.IO.File.Delete(_appEnvironment.WebRootPath + "/BlogImages/" + Path.GetFileName(blog.ImagePath));
				}
				catch (System.IO.IOException e)
				{
					_logger.LogWarning("Failed to delete blog image file. File path: {}", blog.ImagePath);
				}
			}

			_context.Blog.Remove(blog);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool BlogExists(int id)
		{
			return _context.Blog.Any(e => e.BlogId == id);
		}

		private string GetImagePath(IFormFile avatar)
		{
			string fileName = Path.GetFileNameWithoutExtension(avatar.FileName);
			string extension = Path.GetExtension(avatar.FileName);
			fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

			return "BlogImages/" + fileName;
		}
	}
}
