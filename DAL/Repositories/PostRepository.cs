using DAL.Interface.DTO;
using DAL.Interface.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class PostRepository : IPostRepository
	{
		private readonly BlogHostingDbContext _context;

		public PostRepository(BlogHostingDbContext context)
		{
			_context = context;
		}

		public async Task AddPost(Post post)
		{
			_context.Post.Add(post);
			await _context.SaveChangesAsync();
		}

		public async Task AddPostComment(int postId, ApplicationUser author, Comment comment, int parentCommentId)
		{
			var post = await GetPostById(postId);

			comment.Author = author;
			comment.Post = post;
			comment.CreatedDate = DateTime.Now;
			comment.UpdatedDate = comment.UpdatedDate;
			if (parentCommentId != 0)
			{
				comment.ParentCommentId = (int)parentCommentId;

				var parentComment = await _context.Comment.Include(m => m.ChildComments).SingleOrDefaultAsync(m => m.CommentId == parentCommentId);
				if (parentComment != null)
				{
					parentComment.ChildComments.Add(comment);
					_context.Update(parentComment);
				}
			}				

			_context.Comment.Add(comment);
			
			post.Comments.Add(comment);
			_context.Update(post);

			await _context.SaveChangesAsync();
		}

		public async Task AddPostLike(int postId, Like like)
		{
			var post = await GetPostById(postId);
			post.Likes.Add(like);
			_context.Update(post);

			await _context.SaveChangesAsync();
		}

		public async Task AddPostTag(int postId, Tag tag)
		{
			_context.Tag.Add(tag);

			var post = await GetPostById(postId);
			post.Tags.Add(tag);
			_context.Update(post);

			await _context.SaveChangesAsync();
		}

		public async Task DeletePost(int postId)
		{
			var post = await GetPostById(postId);
			_context.Post.Remove(post);
			await _context.SaveChangesAsync();
		}

		public async Task<Post> GetPostById(int id)
		{
			return await _context.Post.FirstOrDefaultAsync(m => m.PostId == id);
		}

		public async Task<Like> GetPostLike(Post post, ApplicationUser user)
		{
			return await _context.Like.FirstOrDefaultAsync(m => m.PostId == post.PostId && m.OwnerId == user.Id);
		}

		public async Task UpdatePost(int postId, string title, string text, string imagePath)
		{
			var post = await GetPostById(postId);

			post.Title = title;
			post.Text = text;
			post.UpdatedDateTime = DateTime.Now;

			if (imagePath != null)
				post.ImagePath = imagePath;

			_context.Update(post);
			await _context.SaveChangesAsync();
		}

		public bool PostExists(int id)
		{
			return _context.Post.Any(e => e.PostId == id);
		}

		public async Task AddPostTags(List<Tag> tags)
		{
			await _context.Tag.AddRangeAsync(tags);
			await _context.SaveChangesAsync();
		}

		public List<Post> GetAllPosts()
		{
			return _context.Post.ToList();
		}

		public async Task DeletePostLike(int postId, int likeId)
		{
			var post = await GetPostById(postId);
			var like = _context.Like.FirstOrDefault(m => m.LikeId == likeId);

			post.Likes.Remove(like);
			_context.Update(post);
			
			await _context.SaveChangesAsync();
		}
	}
}
