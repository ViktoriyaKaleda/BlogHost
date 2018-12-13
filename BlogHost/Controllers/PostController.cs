using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using BlogHost.Models.PostViewModels;
using BlogHost.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BlogHost.Controllers
{
	public class PostController : Controller
    {
		private readonly IPostService _postService;
		private readonly IBlogService _blogService;
		private readonly IAccountService _accountService;
		private readonly IImageService _imageService;
		private readonly IAuthorizationService _authorizationService;

		public PostController(
			IPostService postService,
			IBlogService blogService,
			IAccountService accountService,
			IImageService imageService,
			IAuthorizationService authorizationService)
		{
			_postService = postService;
			_blogService = blogService;
			_accountService = accountService;
			_imageService = imageService;
			_authorizationService = authorizationService;
		}

		// GET: Post/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
				return NotFound();

			var post = await _postService.GetPostById((int)id);
			if (post == null)
				return NotFound();

			TempData["postId"] = post.PostId;

			return View(post);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> PostComment(int id, [Bind("Text")] Comment comment, string parentCommentId)
		{
			id = (int)TempData["postId"];

			var post = await _postService.GetPostById((int)id);
			if (post == null)
				return NotFound();

			if (ModelState.IsValid)
			{
				var author = await _accountService.GetCurrentUser(User);
				int parentId;
				Int32.TryParse(parentCommentId, out parentId);

				await _postService.AddPostComment(post.PostId, author, comment, parentId);
				
				return RedirectToAction(
					nameof(Details),
					new RouteValueDictionary(new { controller = "Post", action = "Details", id = post.PostId }
				));
			}
			return View(post);
		}

		// GET: Post/Create
		[Authorize]
		[HttpGet("[controller]/[action]/{blogId}")]
		public async Task<IActionResult> Create(int? blogId)
		{
			TempData["blogId"] = blogId;

			if (blogId == null)
				return NotFound();

			var blog = await _blogService.GetBlogById((int)blogId);

			if ((await _authorizationService.AuthorizeAsync(User, blog, "OwnerPolicy")).Succeeded)
				return View(new PostCreateViewModel() { Blog = blog });

			return Forbid();
		}

		// POST: Post/Create
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
					Author = await _accountService.GetCurrentUser(User),
					CreatedDateTime = DateTime.Now,
					UpdatedDateTime = DateTime.Now,
					Blog = await _blogService.GetBlogById((int)TempData["blogId"])
				};

				if (viewModel.ImageFile?.FileName != null)
				{
					post.ImagePath = await _imageService.SavePostImage(viewModel.ImageFile);
				}

				if (viewModel.StringTags != null)
				{
					List<Tag> postTags = viewModel.StringTags.Select(m => new Tag { Name = m, PostId = post.PostId }).ToList();
					post.Tags = postTags;
					await _postService.AddPostTags(postTags);
				}

				await _postService.AddPost(post);

				return RedirectToAction(
					nameof(Details),
					"Blog",
					new RouteValueDictionary(new { controller = "Blog", action = "Details", id = post.Blog.BlogId }
				));
			}
			return View(viewModel);
		}

		// GET: Post/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
				return NotFound();

			var post = await _postService.GetPostById((int)id);
			if (post == null)
				return NotFound();

			var blog = await _blogService.GetBlogById(post.Blog.BlogId);

			if ((await _authorizationService.AuthorizeAsync(User, blog, "OwnerPolicy")).Succeeded)
			{
				var viewModel = new PostEditViewModel((int)id, post.Title, post.Text, post.Tags, post.ImagePath, post.Blog);

				return View(viewModel);
			}

			return Forbid();
		}

		// POST: Post/Edit/5
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, PostEditViewModel viewModel)
		{
			if (id != viewModel.PostId)
				return NotFound();

			if (ModelState.IsValid)
			{
				var post = await _postService.GetPostById(id);
				if (post == null)
					return NotFound();

				string imagePath = null;
				if (viewModel.ImageFile?.FileName != null)
					imagePath = await _imageService.SavePostImage(viewModel.ImageFile, post.ImagePath);

				if (viewModel.StringTags != null)
				{
					List<Tag> postTags = viewModel.StringTags.Select(m => new Tag { Name = m, PostId = post.PostId }).ToList();
					post.Tags = postTags;
					await _postService.AddPostTags(postTags);
				}

				await _postService.UpdatePost(id, viewModel.Title, viewModel.Text, imagePath);

				return RedirectToAction(
					nameof(Details),
					new RouteValueDictionary(new { controller = "Post", action = "Details", id = post.PostId }
				));
			}
			return View(viewModel);
		}

		// GET: Post/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			var post = await _postService.GetPostById((int)id);
			if (post == null)
				return NotFound();

			if ((await _authorizationService.AuthorizeAsync(User, post.Blog, "OwnerPolicy")).Succeeded
				|| (await _authorizationService.AuthorizeAsync(User, post.Blog, "ModeratorPolicy")).Succeeded)
				return View(post);

			return Forbid();
		}

		// POST: Post/Delete/5
		[HttpPost, ActionName("Delete")]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var post = await _postService.GetPostById(id);
			_imageService.DeletePostImage(post.ImagePath);
			await _postService.DeletePost(id);
			return RedirectToAction("Index", "Blog");
		}
	}
}