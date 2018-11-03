using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogH.Models;
using BlogHosting.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using BlogHosting.Models;

namespace BlogHosting.Controllers
{
    public class PostsController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private IHostingEnvironment _appEnvironment;

		public PostsController(
				ApplicationDbContext context,
				UserManager<ApplicationUser> userManager,
				IHostingEnvironment appEnvironment
			)
		{
			_context = context;
			_userManager = userManager;
			_appEnvironment = appEnvironment;
		}

		// GET: Posts
		public async Task<IActionResult> Index()
        {
            return View(await _context.Post.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
			if (id == null)
			{
				return NotFound();
			}

			var post = await _context.Post
				.Include(m => m.Comments).ThenInclude(m => m.Author)
				.Include(m => m.Comments).ThenInclude(m => m.ChildComments).ThenInclude(m => m.Author)
				.Include(m => m.Author)
				.Include(m => m.Likes)
				.Include(m => m.Tags)
				.SingleOrDefaultAsync(m => m.PostId == id);

			if (post == null)
            {
                return NotFound();
            }

			TempData["postId"] = post.PostId;

			return View(post);
        }

		private List<Comment> GetChildren(List<Comment> comments, int parentId)
		{
			return comments
					.Where(m => m.ParentCommentId == parentId)
					.Select(c => new Comment
					{
						CommentId = c.CommentId,
						Text = c.Text,
						Post = c.Post,
						Author = c.Author,
						CreatedDate = c.CreatedDate,
						UpdatedDate = c.UpdatedDate,
						ParentCommentId = c.ParentCommentId,
						ChildComments = GetChildren(comments, c.CommentId)
					})
					.ToList();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> PostComment(int id, [Bind("Text")] Comment comment, string parentCommentId)
		{
			id = (int)TempData["postId"];

			Post post = await _context.Post.SingleOrDefaultAsync(m => m.PostId == id);

			if (post == null)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					comment.Author = await _userManager.GetUserAsync(HttpContext.User);
					comment.Post = post;
					comment.CreatedDate = DateTime.Now;
					comment.UpdatedDate = comment.UpdatedDate;
					if (parentCommentId != null)
						comment.ParentCommentId = Int32.Parse(parentCommentId);

					_context.Add(comment);

					if (parentCommentId != null)
					{
						var parentComment = await _context.Comment.Include(m => m.ChildComments).SingleOrDefaultAsync(m => m.CommentId == Int32.Parse(parentCommentId));
						if (parentComment != null)
						{
							parentComment.ChildComments.Add(comment);
							_context.Update(parentComment);
						}
					}

					post.Comments.Add(comment);
					_context.Update(post);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!PostExists(post.PostId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(
					nameof(Details),
					new RouteValueDictionary(new { controller = "Posts", action = "Details", id = post.PostId }
				));
			}
			return View(post);
		}

		// GET: Posts/Create
		[HttpGet("[controller]/[action]/{blogId}")]
		public IActionResult Create(int? blogId)
        {
			TempData["blogId"] = blogId;
			return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Text")] Post post, string[] tags)
        {
            if (ModelState.IsValid)
            {
				post.Author = await _userManager.GetUserAsync(HttpContext.User);
				post.CreatedDateTime = DateTime.Now;
				post.UpdatedDateTime = post.CreatedDateTime;
				post.Blog = await _context.Blog.FirstOrDefaultAsync(b => b.BlogId == (int)TempData["blogId"]);
				if (tags != null)
				{
					List<Tag> postTags = new List<Tag>();
					foreach (var tag in tags)
					{
						Tag postTag = new Tag() { Name = tag, PostId = post.PostId };
						_context.Tag.Add(postTag);
						postTags.Add(postTag);
					}
					post.Tags = postTags;
				}

				_context.Add(post);
                await _context.SaveChangesAsync();
				return RedirectToAction(
					nameof(Details),
					"Blogs",
					new RouteValueDictionary(new { controller = "Blogs", action = "Details", id = post.Blog.BlogId }
				));
			}
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Text")] Post post, string[] tags)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					List<Tag> postTags = new List<Tag>();
					foreach (string tagName in tags)
					{
						Tag tag = new Tag() { Name = tagName };
						postTags.Add(tag);
						_context.Tag.Add(tag);
					}
					post.Tags = postTags;

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
				return RedirectToAction(
					nameof(Details),
					new RouteValueDictionary(new { controller = "Posts", action = "Details", id = post.PostId }
				));
			}
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.PostId == id);
        }
    }
}
