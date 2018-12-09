using DAL.Interface.DTO;
using DAL.Interface.Interfaces;
using Microsoft.EntityFrameworkCore;
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

		public void AddBlogPost(Blog blog, Post post)
		{
			blog.Posts.Add(post);
			UpdateBlog(blog);
		}

		public void DeleteBlog(Blog blog)
		{
			_context.Blog.Remove(blog);
		}

		public async Task<Blog> GetBlogById(int id)
		{
			return await _context.Blog.FirstOrDefaultAsync(m => m.BlogId == id);
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
