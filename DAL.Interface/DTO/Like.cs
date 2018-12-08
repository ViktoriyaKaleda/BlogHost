namespace DAL.Interface.DTO
{
	public class Like
	{
		public int LikeId { get; set; }

		public int PostId { get; set; }

		public virtual Post Post { get; set; }

		public string OwnerId { get; set; }

		public virtual ApplicationUser Owner { get; set; }
	}
}
