using System;
using System.Collections.Generic;

namespace DAL.Interface.DTO
{
	public class Comment
	{
		public int CommentId { get; set; }

		public int PostId { get; set; }

		public virtual Post Post { get; set; }

		public string AuthorId { get; set; }

		public virtual ApplicationUser Author { get; set; }

		public string Text { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime UpdatedDate { get; set; }

		public virtual List<Comment> ChildComments { get; set; }

		public int ParentCommentId { get; set; }
	}
}
