using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogH.Models
{
	public class Comment
	{
		public int CommentId { get; set; }

		public Post Post { get; set; }

		public IdentityUser Author { get; set; }

		public string Text { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime UpdatedDate { get; set; }

		public List<Comment> ChildComments { get; set; }

		public int ParentCommentId { get; set; }
	}
}
