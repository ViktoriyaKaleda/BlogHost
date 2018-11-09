using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace BlogHosting.Services
{
	public interface IImageService
	{
		string GetBlogImagePath(IFormFile blogImage);
		string GetPostImagePath(IFormFile postImage);
		string GetAvatarImagePath(IFormFile avatarImage);
	}

	public class ImageService : IImageService
	{
		private readonly string blogImagesPath;
		private readonly string postImagesPath;
		private readonly string avatarImagesPath;

		public ImageService(string blogImagesPath, string postImagesPath, string avatarImagesPath)
		{
			this.blogImagesPath = blogImagesPath;
			this.postImagesPath = postImagesPath;
			this.avatarImagesPath = avatarImagesPath;
		}

		public string GetAvatarImagePath(IFormFile avatarImage)
		{
			return GetImagePath(avatarImage, avatarImagesPath);
		}

		public string GetBlogImagePath(IFormFile blogImage)
		{
			return GetImagePath(blogImage, blogImagesPath);
		}

		public string GetPostImagePath(IFormFile postImage)
		{
			return GetImagePath(postImage, postImagesPath);
		}

		private string GetImagePath(IFormFile image, string folderName)
		{
			string fileName = Path.GetFileNameWithoutExtension(image.FileName);
			string extension = Path.GetExtension(image.FileName);
			fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

			return folderName + "/" + fileName;
		}
	}
}
