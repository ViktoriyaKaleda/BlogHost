using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BlogHost.Services
{
	public interface IImageService
	{
		Task<string> SaveBlogImage(IFormFile blogImage, string oldPath = null);
		Task<string> SavePostImage(IFormFile postImage, string oldPath = null);
		Task<string> SaveAvatarImage(IFormFile avatarImage, string oldPath = null);

		void DeleteBlogImage(string path);
		void DeletePostImage(string path);
		void DeleteAvatarImage(string path);
	}
}
