using System;
using System.Collections.Generic;

namespace BLL.Interface.Entities
{
	public class Comment
	{
		public int CommentId { get; set; }

		public int PostId { get; set; }

		public string AuthorId { get; set; }

		public ApplicationUser Author { get; set; }

		public string Text { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime UpdatedDate { get; set; }

		public List<Comment> ChildComments { get; set; }

		public int ParentCommentId { get; set; }

		public Comment()
		{
			ChildComments = new List<Comment>();
		}
	}
}
