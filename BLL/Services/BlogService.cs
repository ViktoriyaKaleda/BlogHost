using AutoMapper;
using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using DAL.Interface.Interfaces;
using System;
using System.Collections.Generic;
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

			_repository.AddBlog(Mapper.Map<DAL.Interface.DTO.Blog>(blog));

			await _repository.Save();
		}

		public async Task AddBlogModerator(Blog blog, ApplicationUser user)
		{
			_repository.AddBlogModerator(Mapper.Map<DAL.Interface.DTO.Blog>(blog), Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user));
			_repository.UpdateBlog(Mapper.Map<DAL.Interface.DTO.Blog>(blog));
			await _repository.Save();
		}

		public async Task DeleteBlog(int id)
		{
			var blog = Mapper.Map<Blog>(await _repository.GetBlogById(id));
			var blogModerators = Mapper.Map<List<ApplicationUser>>(await _repository.GetBlogModerators(id));

			if (blogModerators.Count != 0)
			{
				foreach (var blogModerator in blogModerators)
				{
					await _repository.DeleteBlogModerator(Mapper.Map<DAL.Interface.DTO.Blog>(blog),
						Mapper.Map<DAL.Interface.DTO.ApplicationUser>(blogModerator));
				}
			}

			_repository.DeleteBlog(Mapper.Map<DAL.Interface.DTO.Blog>(blog));
		}

		public async Task DeleteBlogModerator(Blog blog, ApplicationUser user)
		{
			await _repository.DeleteBlogModerator(Mapper.Map<DAL.Interface.DTO.Blog>(blog),
				Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user));
			await _repository.Save();
		}

		public async Task<Blog> GetBlogById(int id)
		{
			return Mapper.Map<Blog>(await _repository.GetBlogById(id));
		}

		public async Task<ApplicationUser> GetBlogModeratorById(int blogId, string id)
		{
			var blog = Mapper.Map<Blog>(await _repository.GetBlogById(blogId));

			return Mapper.Map<ApplicationUser>(_repository.GetBlogModeratorById(Mapper.Map<DAL.Interface.DTO.Blog>(blog), id));
		}

		public async Task<BlogStyle> GetBlogStyleById(int id)
		{
			return Mapper.Map<BlogStyle>(await _repository.GetBlogStyleById(id));
		}

		public async Task UpdateBlog(Blog blog, string name, string description, int blogStyleId)
		{
			blog.BlogName = name;
			blog.Description = description;
			blog.UpdatedDateTime = DateTime.Now;

			var style = await GetBlogStyleById(blogStyleId);
			blog.BlogStyle = style;

			_repository.UpdateBlog(Mapper.Map<DAL.Interface.DTO.Blog>(blog));
			await _repository.Save();
		}
	}
}
