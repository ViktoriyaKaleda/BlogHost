namespace DAL.Interface.DTO
{
	public class BlogModerator
	{
		public int BlogId { get; set; }
		public virtual Blog Blog { get; set; }

		public string ModeratorId { get; set; }
		public virtual ApplicationUser Moderator { get; set; }
	}
}
