using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogH.Models;
using BlogHosting.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using BlogHosting.Models;
using Microsoft.Extensions.Logging;
using BlogHosting.Models.PostViewModels;
using System.IO;
using Microsoft.AspNetCore.Http;
using CodeKicker.BBCode;
using BlogHosting.Services;

namespace BlogHosting.Controllers
{
	public class PostsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private IHostingEnvironment _appEnvironment;
		private readonly IAuthorizationService _authorizationService;
		private readonly ILogger _logger;
		private readonly IImageService _imageService;

		public PostsController(
				ApplicationDbContext context,
				UserManager<ApplicationUser> userManager,
				IHostingEnvironment appEnvironment,
				IAuthorizationService authorizationService,
				ILogger<PostsController> logger,
				IImageService imageService
			)
		{
			_context = context;
			_userManager = userManager;
			_appEnvironment = appEnvironment;
			_authorizationService = authorizationService;
			_logger = logger;
			_imageService = imageService;
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
				.SingleOrDefaultAsync(m => m.PostId == id);

			if (post == null)
			{
				return NotFound();
			}

			//post.Text = BBCode.ToHtml(post.Text);

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

		[HttpPost("[Controller]/Details/{id}/DeleteComment")]
		[Authorize]
		public async Task<IActionResult> DeleteComment(int id, [FromBody]int? commentId)
		{
			var comment = await _context.Comment.FirstOrDefaultAsync(m => m.CommentId == commentId);
			var postId = comment.Post.PostId;

			if (comment == null)
				return NotFound();

			if (!(await _authorizationService.AuthorizeAsync(User, comment.Post.Blog, "OwnerPolicy")).Succeeded
				&& !(await _authorizationService.AuthorizeAsync(User, comment.Post.Blog, "ModeratorPolicy")).Succeeded)
				return Forbid();

			await DeleteChildCommentsParant(comment);

			_context.Comment.Remove(comment);
			await _context.SaveChangesAsync();

			return PartialView("~/Views/Posts/CommentPartial.cshtml", 
				await _context.Comment.Where(m => m.Post.PostId == postId && m.ParentCommentId == 0).ToListAsync());
		}

		private async Task DeleteChildCommentsParant(Comment comment)
		{
			foreach (var c in comment.ChildComments)
			{
				c.ParentCommentId = 0;
				_context.Comment.Update(c);
			}
			await _context.SaveChangesAsync();
		}

		// GET: Posts/Create
		[Authorize]
		[HttpGet("[controller]/[action]/{blogId}")]
		public async Task<IActionResult> Create(int? blogId)
		{
			TempData["blogId"] = blogId;

			var blog = await _context.Blog.FirstOrDefaultAsync(m => m.BlogId == blogId);

			if ((await _authorizationService.AuthorizeAsync(User, blog, "OwnerPolicy")).Succeeded)
				return View(new PostCreateViewModel() { Blog = blog });

			return Forbid();
		}

		// POST: Posts/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(PostCreateViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var post = new Post()
				{
					Title = viewModel.Title,
					Text = viewModel.Text,
					Author = await _userManager.GetUserAsync(HttpContext.User),
					CreatedDateTime = DateTime.Now,
					UpdatedDateTime = DateTime.Now,
					Blog = await _context.Blog.FirstOrDefaultAsync(b => b.BlogId == (int)TempData["blogId"])
				};

				if (viewModel.StringTags != null)
				{
					List<Tag> postTags = new List<Tag>();
					foreach (var tag in viewModel.StringTags)
					{
						Tag postTag = new Tag() { Name = tag, PostId = post.PostId };
						_context.Tag.Add(postTag);
						postTags.Add(postTag);
					}
					post.Tags = postTags;
				}

				if (viewModel.ImageFile?.FileName != null)
				{
					string path = _imageService.GetPostImagePath(viewModel.ImageFile);

					post.ImagePath = "~/" + path;

					using (var fileStream = new FileStream(_appEnvironment.WebRootPath + "/" + path, FileMode.Create))
					{
						await viewModel.ImageFile.CopyToAsync(fileStream);
					}
				}

				_context.Add(post);
				await _context.SaveChangesAsync();
				return RedirectToAction(
					nameof(Details),
					"Blogs",
					new RouteValueDictionary(new { controller = "Blogs", action = "Details", id = post.Blog.BlogId }
				));
			}
			return View(viewModel);
		}

		// GET: Posts/Edit/5
		[Authorize]
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

			var blog = await _context.Blog.FirstOrDefaultAsync(m => m.BlogId == post.Blog.BlogId);

			if ((await _authorizationService.AuthorizeAsync(User, blog, "OwnerPolicy")).Succeeded)
			{
				var viewModel = new PostEditViewModel((int)id, post.Title, post.Text, post.Tags, post.ImagePath, post.Blog);

				return View(viewModel);
			}

			return Forbid();
		}

		// POST: Posts/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, PostEditViewModel viewModel)
		{
			if (id != viewModel.PostId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				var post = _context.Post.FirstOrDefault(m => m.PostId == id);
				if (post == null)
					return NotFound();

				post.Title = viewModel.Title;
				post.Text = viewModel.Text;
				post.UpdatedDateTime = DateTime.Now;

				if (viewModel.ImageFile?.FileName != null)
				{
					if (post.ImagePath != null)
					{
						try
						{
							System.IO.File.Delete(_appEnvironment.WebRootPath + "/PostImages/" + Path.GetFileName(post.ImagePath));
						}
						catch (System.IO.IOException e)
						{
							_logger.LogWarning("Failed to delete post image file. File path: {}", post.ImagePath);
						}
					}

					string path = _imageService.GetPostImagePath(viewModel.ImageFile);

					post.ImagePath = "~/" + path;

					using (var fileStream = new FileStream(_appEnvironment.WebRootPath + "/" + path, FileMode.Create))
					{
						await viewModel.ImageFile.CopyToAsync(fileStream);
					}
				}

				List<Tag> postTags = new List<Tag>();
				if (viewModel.StringTags != null)
				{
					foreach (string tagName in viewModel.StringTags)
					{
						Tag tag = new Tag() { Name = tagName };
						postTags.Add(tag);
						_context.Tag.Add(tag);
					}
				}
				
				post.Tags = postTags;

				_context.Update(post);
				await _context.SaveChangesAsync();

				return RedirectToAction(
					nameof(Details),
					new RouteValueDictionary(new { controller = "Posts", action = "Details", id = post.PostId }
				));
			}
			return View(viewModel);
		}

		// GET: Posts/Delete/5
		[Authorize]
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

			if ((await _authorizationService.AuthorizeAsync(User, post.Blog, "OwnerPolicy")).Succeeded
				|| (await _authorizationService.AuthorizeAsync(User, post.Blog, "ModeratorPolicy")).Succeeded)
				return View(post);

			return Forbid();
		}

		// POST: Posts/Delete/5
		[HttpPost, ActionName("Delete")]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var post = await _context.Post.FindAsync(id);
			_context.Post.Remove(post);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index), "Blogs");
		}

		private bool PostExists(int id)
		{
			return _context.Post.Any(e => e.PostId == id);
		}
	}
}
