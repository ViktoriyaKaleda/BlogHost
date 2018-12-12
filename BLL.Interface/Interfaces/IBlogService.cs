using BLL.Interface.Entities;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BLL.Interface.Interfaces
{
	public interface IBlogService
	{
		Task<Blog> GetBlogById(int id);

		Task<ApplicationUser> GetBlogModeratorById(int blogId, string id);

		Task AddBlogModerator(Blog blog, ApplicationUser user);

		Task DeleteBlogModerator(Blog blog, ApplicationUser user);

		Task<BlogStyle> GetBlogStyleById(int id);

		Task AddBlog(Blog blog, int blogStyleId);

		Task UpdateBlog(Blog blog, string name, string description, int blogStyleId);

		Task DeleteBlog(int id);
	}
}
