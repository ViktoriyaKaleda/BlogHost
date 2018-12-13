using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace BlogHost.Hubs
{
	public class ChatHub : Hub
	{
		private readonly IPostService _postService;
		private readonly IAccountService _accountService;

		public ChatHub(IPostService postService, IAccountService accountService)
		{
			_postService = postService;
			_accountService = accountService;
		}

		public async Task SetLike(string postId)
		{
			int id = Int32.Parse(postId);

			var owner = await _accountService.GetCurrentUser(Context.GetHttpContext().User);

			var post = await _postService.GetPostById(id);

			var like = await _postService.GetPostLike(post, owner);

			int likesNumber;

			if (like == null)
			{
				Like newLike = new Like() { Owner = owner, Post = post };
				await _postService.AddPostLike(post.PostId, newLike);

				likesNumber = (await _postService.GetPostById(id)).Likes.Count;
			}

			else
			{
				await _postService.DeletePostLike(post.PostId, like.LikeId);

				likesNumber = (await _postService.GetPostById(id)).Likes.Count;
			}
			
			await Clients.All.SendAsync("setLike", likesNumber.ToString(), postId);
		}
	}
}
