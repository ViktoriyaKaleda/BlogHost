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
using System.Net.Http;
using System.Net;

namespace BlogHosting.Controllers
{
	public class BlogsController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHostingEnvironment _appEnvironment;
		private readonly IAuthorizationService _authorizationService;

		public BlogsController(
				ApplicationDbContext context,
				UserManager<ApplicationUser> userManager,
				IHostingEnvironment appEnvironment,
				IAuthorizationService authorizationService
			)
		{
			_context = context;
			_userManager = userManager;
			_appEnvironment = appEnvironment;
			_authorizationService = authorizationService;
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
				.Include(m => m.Author)
				.Include(m => m.Posts).ThenInclude(m => m.Comments)
				.Include(m => m.Posts).ThenInclude(m => m.Likes)
				.Include(m => m.Posts).ThenInclude(m => m.Author)
				.Include(m => m.Posts).ThenInclude(m => m.Tags)
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
				CommentsNumber = _context.Comment.Where(c => c.Post == m).Count()
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
				.Include(m => m.Tags)
				.Include(m => m.Author)
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
		public async Task<IActionResult> Create([Bind("BlogId,BlogName,Description,CreatedDateTime,UpdatedDateTime")] Blog blog)
        {
            if (ModelState.IsValid)
            {
				blog.Author = await _userManager.GetUserAsync(HttpContext.User);
				blog.CreatedDateTime = DateTime.Now;
				blog.UpdatedDateTime = blog.CreatedDateTime;

				_context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

		// GET: Blogs/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.Include(m => m.Author).FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }

			var isOwner = await _authorizationService.AuthorizeAsync(User, blog, "OwnerPolicy");
			if (isOwner.Succeeded)
			{
				var viewModel = new BlogEditViewModel()
				{
					BlogId = (int)id,
					BlogName = blog.BlogName,
					Description = blog.Description,
					ImagePath = blog.ImagePath,
					Moderators = blog.BlogModerators
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
		public async Task<IActionResult> Edit(int id, [Bind("BlogId,BlogName,Description,CreatedDateTime,UpdatedDateTime")] Blog blog)
        {
            if (id != blog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

		// GET: Blogs/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			var blog = await _context.Blog.Include(m => m.Author).FirstOrDefaultAsync(m => m.BlogId == id);
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
