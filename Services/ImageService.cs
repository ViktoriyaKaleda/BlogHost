using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BlogHosting.Services
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

	public class ImageService : IImageService
	{
		private readonly string blogImagesPath;
		private readonly string postImagesPath;
		private readonly string avatarImagesPath;
		private readonly IHostingEnvironment _appEnvironment;
		private readonly ILogger<ImageService> _logger;
		private readonly IConfiguration _configuration;

		public ImageService(
			IHostingEnvironment appEnvironment,
			ILogger<ImageService> logger,
			IConfiguration configuration)
		{
			_appEnvironment = appEnvironment;
			_logger = logger;
			_configuration = configuration;

			this.blogImagesPath = _configuration.GetValue<string>("ImagesFolders:Blogs");
			this.postImagesPath = _configuration.GetValue<string>("ImagesFolders:Posts");
			this.avatarImagesPath = _configuration.GetValue<string>("ImagesFolders:Avatars");
		}				

		#region Save methods

		public async Task<string> SaveAvatarImage(IFormFile avatarImage, string oldPath = null)
		{
			if (oldPath != null)
				DeleteFile(oldPath, avatarImagesPath);

			string newImagePath = GetImagePath(avatarImage, avatarImagesPath);
			using (var fileStream = new FileStream(_appEnvironment.WebRootPath + "/" + newImagePath, FileMode.Create))
			{
				await avatarImage.CopyToAsync(fileStream);
			}

			return "~/" + newImagePath;
		}

		public async Task<string> SaveBlogImage(IFormFile blogImage, string oldPath = null)
		{
			if (oldPath != null)
				DeleteFile(oldPath, avatarImagesPath);

			string newImagePath = GetImagePath(blogImage, blogImagesPath);
			using (var fileStream = new FileStream(_appEnvironment.WebRootPath + "/" + newImagePath, FileMode.Create))
			{
				await blogImage.CopyToAsync(fileStream);
			};

			return "~/" + newImagePath;
		}

		public async Task<string> SavePostImage(IFormFile postImage, string oldPath = null)
		{
			if (oldPath != null)
				DeleteFile(oldPath, avatarImagesPath);

			string newImagePath = GetImagePath(postImage, postImagesPath);
			using (var fileStream = new FileStream(_appEnvironment.WebRootPath + "/" + newImagePath, FileMode.Create))
			{
				await postImage.CopyToAsync(fileStream);
			}

			return "~/" + newImagePath;
		}
		#endregion

		#region Delete methods

		public void DeleteAvatarImage(string path)
		{
			DeleteFile(path, avatarImagesPath);
		}

		public void DeleteBlogImage(string path)
		{
			DeleteFile(path, blogImagesPath);
		}

		public void DeletePostImage(string path)
		{
			DeleteFile(path, postImagesPath);
		}

		#endregion

		#region Private methods

		private string GetImagePath(IFormFile image, string folderName)
		{
			string fileName = Path.GetFileNameWithoutExtension(image.FileName);
			string extension = Path.GetExtension(image.FileName);
			fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

			return folderName + "/" + fileName;
		}

		private void DeleteFile(string path, string folderName)
		{
			try
			{
				System.IO.File.Delete(_appEnvironment.WebRootPath + $"/{folderName}/" + Path.GetFileName(path));
			}
			catch (System.IO.IOException e)
			{
				_logger.LogWarning("Failed to delete blog image file. File path: {}", path);
			}
		}
		#endregion
	}
}
