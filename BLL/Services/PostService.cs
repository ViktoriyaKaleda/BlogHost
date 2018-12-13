using AutoMapper;
using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using DAL.Interface.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public async Task AddPost(Post post)
		{
			if (post.ImagePath == null)
				post.ImagePath = post.Blog.BlogStyle.DefaultImagePath;

			await _repository.AddPost(Mapper.Map<DAL.Interface.DTO.Post>(post));
		}

		public async Task AddPostComment(int postId, ApplicationUser author, Comment comment, int parentCommentId)
		{
			await _repository.AddPostComment(postId, Mapper.Map<DAL.Interface.DTO.ApplicationUser>(author),
				Mapper.Map<DAL.Interface.DTO.Comment>(comment), parentCommentId);
		}

		public async Task AddPostLike(int postId, Like like)
		{
			await _repository.AddPostLike(postId, Mapper.Map<DAL.Interface.DTO.Like>(like));
		}

		public async Task AddPostTags(List<Tag> tags)
		{
			await _repository.AddPostTags(Mapper.Map<List<DAL.Interface.DTO.Tag>>(tags));
		}

		public async Task DeletePost(int id)
		{
			await _repository.DeletePost(id);
		}

		public async Task DeletePostLike(int postId, int likeId)
		{
			await _repository.DeletePostLike(postId, likeId);
		}

		public List<Post> GetAllPosts()
		{
			return Mapper.Map<List<Post>>(_repository.GetAllPosts());
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

		public bool PostExists(int id)
		{
			return _repository.PostExists(id);
		}

		public async Task UpdatePost(int id, string title, string text, string imagePath)
		{
			await _repository.UpdatePost(id, title, text, imagePath);
		}
	}
}
