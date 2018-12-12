using DAL.Interface.DTO;
using DAL.Interface.Interfaces;
using Microsoft.EntityFrameworkCore;
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

		public void AddBlog(Blog blog)
		{
			_context.Blog.Add(blog);
		}

		public void AddBlogModerator(Blog blog, ApplicationUser user)
		{
			blog.BlogModerators.Add(new BlogModerator() { Blog = blog, Moderator = user });
		}

		public void AddBlogPost(Blog blog, Post post)
		{
			blog.Posts.Add(post);
			UpdateBlog(blog);
		}

		public void DeleteBlog(Blog blog)
		{
			_context.Blog.Remove(blog);
		}

		public async Task DeleteBlogModerator(Blog blog, ApplicationUser user)
		{
			var blogModerator = await _context.BlogModerator.FirstOrDefaultAsync(m => m.BlogId == blog.BlogId && m.ModeratorId == user.Id);
			_context.BlogModerator.Remove(blogModerator);
		}

		public async Task<Blog> GetBlogById(int id)
		{
			return await _context.Blog.FirstOrDefaultAsync(m => m.BlogId == id);
		}

		public ApplicationUser GetBlogModeratorById(Blog blog, string id)
		{
			return blog.BlogModerators.Select(m => m.Moderator).FirstOrDefault(m => m.Id == id);
		}

		public async Task<List<ApplicationUser>> GetBlogModerators(int blogId)
		{
			var blog = await _context.Blog.FirstOrDefaultAsync(m => m.BlogId == blogId);
			return blog.BlogModerators.Select(m => m.Moderator).ToList();
		}

		public async Task<BlogStyle> GetBlogStyleById(int styleId)
		{
			return await _context.BlogStyle.FirstOrDefaultAsync(m => m.BlogStyleId == styleId);
		}

		public async Task Save()
		{
			await _context.SaveChangesAsync();
		}

		public void UpdateBlog(Blog blog)
		{
			_context.Blog.Update(blog);
		}
	}
}
