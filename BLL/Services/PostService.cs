using AutoMapper;
using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using DAL.Interface.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class PostService : IPostService
	{
		private readonly IPostRepository _repository;

		public PostService(IPostRepository repository)
		{
			_repository = repository;
		}

		public Task AddPost(Post post, IFormFile image)
		{
			throw new NotImplementedException();
		}

		public Task AddPostComment(int postId, ApplicationUser author, Comment comment, string parentCommentId)
		{
			throw new NotImplementedException();
		}

		public Task DeletePost(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<Post>> GetAllPosts()
		{
			throw new NotImplementedException();
		}

		public async Task<Post> GetPostById(int id)
		{
			return Mapper.Map<Post>(await _repository.GetPostById(id));
		}

		public async Task<Like> GetPostLike(Post post, ApplicationUser user)
		{
			return Mapper.Map<Like>(await _repository.GetPostLike(Mapper.Map<DAL.Interface.DTO.Post>(post),
				Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user)));
		}

		public Task UpdatePost(int id, string title, string text, IFormFile image, List<Tag> tags)
		{
			throw new NotImplementedException();
		}
	}
}
