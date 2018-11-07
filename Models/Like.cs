using BlogHosting.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogH.Models
{
	public class Like
	{
		public int LikeId { get; set; }

		public virtual Post Post { get; set; }

		public virtual ApplicationUser Owner { get; set; }
	}
}
