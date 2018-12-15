using DAL.Interface.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interface.Interfaces
{
	public interface IBlogRepository
	{
		Task<Blog> GetBlogById(int blogId);

		IQueryable<Blog> GetAllBlogs();

		Task<BlogStyle> GetBlogStyleById(int styleId);

		Task<List<BlogStyle>> GetAllBlogStyles();

		Task<ApplicationUser> GetBlogModeratorById(int blogId, string id);

		Task<List<ApplicationUser>> GetAllBlogModerators(int blogId);

		Task AddBlog(Blog blog);

		Task UpdateBlog(int blogId, string name, string description, int blogStyleId, string imagePath);

		Task DeleteBlog(int blogId);

		void AddBlogPost(Blog blog, Post post);

		Task AddBlogModerator(int blogId, ApplicationUser user);

		Task DeleteBlogModerator(int blogId, ApplicationUser user);

		bool BlogExists(int id);
	}
}
