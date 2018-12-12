using BLL.Interface.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Interface.Interfaces
{
	public interface IBlogService
	{
		List<Blog> GetAllBlogs();

		Task<Blog> GetBlogById(int id);

		Task<List<ApplicationUser>> GetAllBlogModerators(int blogId);

		Task<ApplicationUser> GetBlogModeratorById(int blogId, string id);

		Task AddBlogModerator(Blog blog, ApplicationUser user);

		Task DeleteBlogModerator(Blog blog, ApplicationUser user);

		Task<List<BlogStyle>> GetAllBlogStyles();

		Task<BlogStyle> GetBlogStyleById(int id);

		Task AddBlog(Blog blog, int blogStyleId);

		Task UpdateBlog(int blogId, string name, string description, int blogStyleId, string ImagePath);

		Task DeleteBlog(int id);
	}
}
