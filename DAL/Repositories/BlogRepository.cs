using DAL.Interface.DTO;
using DAL.Interface.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class BlogRepository : IBlogRepository
	{
		private readonly BlogHostingDbContext _context;

		public BlogRepository(BlogHostingDbContext context)
		{
			_context = context;
		}

		public async void AddBlog(Blog blog)
		{
			_context.Blog.Add(blog);
			await _context.SaveChangesAsync();
		}

		public async void AddBlogModerator(Blog blog, ApplicationUser user)
		{
			blog.BlogModerators.Add(new BlogModerator() { Blog = blog, Moderator = user });
			_context.Update(blog);
			await _context.SaveChangesAsync();
		}

		public async void AddBlogPost(Blog blog, Post post)
		{
			blog.Posts.Add(post);
			_context.Update(blog);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteBlog(int blogId)
		{
			var blog = await GetBlogById(blogId);
			var blogModerators = await GetAllBlogModerators(blogId);

			if (blogModerators.Count != 0)
			{
				foreach (var blogModerator in blogModerators)
				{
					await DeleteBlogModerator(blog, blogModerator);
				}
			}

			_context.Blog.Remove(blog);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteBlogModerator(Blog blog, ApplicationUser user)
		{
			var blogModerator = await _context.BlogModerator.FirstOrDefaultAsync(m => m.BlogId == blog.BlogId && m.ModeratorId == user.Id);
			_context.BlogModerator.Remove(blogModerator);
			await _context.SaveChangesAsync();
		}

		public IQueryable<Blog> GetAllBlogs()
		{
			return _context.Blog;
		}

		public async Task<List<BlogStyle>> GetAllBlogStyles()
		{
			return await _context.BlogStyle.ToListAsync();
		}

		public async Task<Blog> GetBlogById(int id)
		{
			return await _context.Blog.FirstOrDefaultAsync(m => m.BlogId == id);
		}

		public ApplicationUser GetBlogModeratorById(Blog blog, string id)
		{
			return blog.BlogModerators.Select(m => m.Moderator).FirstOrDefault(m => m.Id == id);
		}

		public async Task<List<ApplicationUser>> GetAllBlogModerators(int blogId)
		{
			var blog = await _context.Blog.FirstOrDefaultAsync(m => m.BlogId == blogId);
			return blog.BlogModerators.Select(m => m.Moderator).ToList();
		}

		public async Task<BlogStyle> GetBlogStyleById(int styleId)
		{
			return await _context.BlogStyle.FirstOrDefaultAsync(m => m.BlogStyleId == styleId);
		}

		public async Task UpdateBlog(int blogId, string name, string description, int blogStyleId, string imagePath)
		{
			var blog = await _context.Blog.FirstOrDefaultAsync(m => m.BlogId == blogId);

			blog.BlogName = name;
			blog.Description = description;
			blog.UpdatedDateTime = DateTime.Now;

			if (imagePath != null)
				blog.ImagePath = imagePath;

			var style = await _context.BlogStyle.FirstOrDefaultAsync(m => m.BlogStyleId == blogStyleId);
			blog.BlogStyle = style;
			
			_context.Update(blog);
			await _context.SaveChangesAsync();
		}
	}
}
