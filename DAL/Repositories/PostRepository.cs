using DAL.Interface.DTO;
using DAL.Interface.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
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

		public void AddPost(Post post)
		{
			_context.Post.Add(post);
		}

		public async Task AddPostComment(Post post, Comment comment, int parentCommentId)
		{
			_context.Comment.Add(comment);

			if (parentCommentId != 0)
			{
				var parentComment = await _context.Comment.Include(m => m.ChildComments).SingleOrDefaultAsync(m => m.CommentId == parentCommentId);
				if (parentComment != null)
				{
					parentComment.ChildComments.Add(comment);
					_context.Update(parentComment);
				}
			}

			post.Comments.Add(comment);
			_context.Update(post);
		}

		public void AddPostLike(Post post, Like like)
		{
			_context.Like.Add(like);
			post.Likes.Add(like);
			_context.Update(post);
		}

		public void AddPostTag(Post post, Tag tag)
		{
			_context.Tag.Add(tag);
			post.Tags.Add(tag);
			_context.Update(post);
		}

		public void DeletePost(Post post)
		{
			_context.Post.Remove(post);
		}

		public async Task<Post> GetPostById(int id)
		{
			return await _context.Post.FirstOrDefaultAsync(m => m.PostId == id);
		}

		public async Task Save()
		{
			await _context.SaveChangesAsync();
		}

		public void UpdatePost(Post post)
		{
			_context.Post.Update(post);
		}
	}
}
