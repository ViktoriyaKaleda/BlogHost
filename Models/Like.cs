using Microsoft.AspNetCore.Identity;

namespace BlogH.Models
{
	public class Like
	{
		public int LikeId { get; set; }

		public Post Post { get; set; }

		public IdentityUser Owner { get; set; }
	}
}
