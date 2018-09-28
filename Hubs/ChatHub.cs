using BlogH.Models;
using BlogHosting.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
	public class ChatHub : Hub
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public ChatHub(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task SetLike(string postId)
		{
			int id = Int32.Parse(postId);

			var owner = await _userManager.GetUserAsync(Context.GetHttpContext().User);
			var post = await _context.Post.SingleOrDefaultAsync(m => m.PostId == id);

			Like like = await _context.Like.SingleOrDefaultAsync(m => m.Post == post && m.Owner == owner);

			int likesNumber;

			if (like == null)
			{
				Like newLike = new Like() { Owner = owner, Post = post };
				_context.Like.Add(newLike);
				post.Likes = new List<Like>() { newLike };
				_context.Update(post);
				await _context.SaveChangesAsync();

				likesNumber = _context.Like.Where(m => m.Post == post).Count();
			}
			else
			{
				post.Likes.Remove(like);
				_context.Update(post);

				_context.Like.Remove(like);
				await _context.SaveChangesAsync();

				likesNumber = _context.Like.Where(m => m.Post == post).Count();
			}

			//return Clients.All.updateLikeCount(likesNumber);
			await Clients.All.SendAsync("setLike", likesNumber.ToString());
		}
	}
}
