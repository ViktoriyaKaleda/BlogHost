using DAL.Interface.DTO;
using System.Threading.Tasks;

namespace DAL.Interface.Interfaces
{
	public interface IBlogRepository
	{
		Task<Blog> GetBlogById(int id);

		void AddBlog(Blog blog);

		void UpdateBlog(Blog blog);

		void DeleteBlog(Blog blog);

		void AddBlogPost(Blog blog, Post post);

		Task Save();
	}
}
