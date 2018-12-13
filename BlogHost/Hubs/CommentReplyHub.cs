using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BlogHost.Hubs
{
	public class CommentReplyHub : Hub
	{
		public async Task ShowReply(string commentId, string username)
		{
			await Clients.All.SendAsync("ShowReply", commentId, username);
		}
	}
}
