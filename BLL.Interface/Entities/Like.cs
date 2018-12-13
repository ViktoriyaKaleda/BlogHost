namespace BLL.Interface.Entities
{
	public class Like
	{
		public int LikeId { get; set; }

		public int PostId { get; set; }

		public Post Post { get; set; }

		public string OwnerId { get; set; }

		public ApplicationUser Owner { get; set; }
	}
}
