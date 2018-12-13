using AutoMapper;
using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using DAL.Interface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class BlogService : IBlogService
	{
		private readonly IBlogRepository _repository;

		public BlogService(IBlogRepository repository)
		{
			_repository = repository;
		}

		public async Task AddBlog(Blog blog, int blogStyleId)
		{
			var style = Mapper.Map<BlogStyle>(await _repository.GetBlogStyleById(blogStyleId));

			blog.BlogStyle = style;

			if (blog.ImagePath == null)
				blog.ImagePath = style.DefaultImagePath;

			_repository.AddBlog(Mapper.Map<DAL.Interface.DTO.Blog>(blog));
		}

		public async Task AddBlogModerator(int blogId, ApplicationUser user)
		{
			await _repository.AddBlogModerator(blogId, Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user));
		}

		public async Task DeleteBlog(int id)
		{
			await _repository.DeleteBlog(id);
		}

		public async Task DeleteBlogModerator(int blogId, ApplicationUser user)
		{
			await _repository.DeleteBlogModerator(blogId, Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user));
		}

		public async Task<List<ApplicationUser>> GetAllBlogModerators(int blogId)
		{
			return Mapper.Map<List<ApplicationUser>>(await _repository.GetAllBlogModerators(blogId));
		}

		public List<Blog> GetAllBlogs()
		{
			return Mapper.Map<List<Blog>>(_repository.GetAllBlogs().ToList());
		}

		public async Task<List<BlogStyle>> GetAllBlogStyles()
		{
			return Mapper.Map<List<BlogStyle>>(await _repository.GetAllBlogStyles());
		}

		public async Task<Blog> GetBlogById(int id)
		{
			return Mapper.Map<Blog>(await _repository.GetBlogById(id));
		}

		public async Task<ApplicationUser> GetBlogModeratorById(int blogId, string id)
		{
			return Mapper.Map<ApplicationUser>(await _repository.GetBlogModeratorById(blogId, id));
		}

		public async Task<BlogStyle> GetBlogStyleById(int id)
		{
			return Mapper.Map<BlogStyle>(await _repository.GetBlogStyleById(id));
		}
		
		public bool BlogExists(int id)
		{
			return _repository.BlogExists(id);
		}

		public async Task UpdateBlog(int blogId, string name, string description, int blogStyleId, string imagePath)
		{
			await _repository.UpdateBlog(blogId, name, description, blogStyleId, imagePath);
		}
	}
}
