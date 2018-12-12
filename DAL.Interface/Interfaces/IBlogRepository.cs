using DAL.Interface.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interface.Interfaces
{
	public interface IBlogRepository
	{
		Task<Blog> GetBlogById(int blogId);

		Task<BlogStyle> GetBlogStyleById(int styleId);

		ApplicationUser GetBlogModeratorById(Blog blog, string id);

		Task<List<ApplicationUser>> GetBlogModerators(int blogId);

		void AddBlog(Blog blog);

		void UpdateBlog(Blog blog);

		void DeleteBlog(Blog blog);

		void AddBlogPost(Blog blog, Post post);

		void AddBlogModerator(Blog blog, ApplicationUser user);

		Task DeleteBlogModerator(Blog blog, ApplicationUser user);

		Task Save();
	}
}
