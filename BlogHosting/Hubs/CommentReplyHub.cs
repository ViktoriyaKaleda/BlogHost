using BlogH.Models;
using BlogHosting.Data;
using BlogHosting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogHosting.Hubs
{
	public class CommentReplyHub : Hub
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public CommentReplyHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task ShowReply(string commentId, string username)
		{
			await Clients.All.SendAsync("ShowReply", commentId, username);
		}
	}
}
