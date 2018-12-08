using BLL.Interface.Entities;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BLL.Interface.Interfaces
{
	public interface IBlogService
	{
		Task<Blog> GetBlogById(int id);

		ApplicationUser GetBlogModeratorById(string id);

		Task AddBlogModerator(ApplicationUser user);

		Task DeleteBlogModerator(ApplicationUser user);

		Task<BlogStyle> GetBlogStyleById(int id);

		Task AddBlog(Blog blog, int blogStyleId, IFormFile image);

		Task UpdateBlog(int id, string name, string description, int BlogStyleId, IFormFile image);

		Task DeleteBlog(int id);
	}
}
