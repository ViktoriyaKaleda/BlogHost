using BLL.Interface.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Interface.Interfaces
{
	public interface IPostService
	{
		Task<Post> GetPostById(int id);

		Task<Like> GetPostLike(Post post, ApplicationUser user);

		List<Post> GetAllPosts();

		Task AddPost(Post post);

		Task AddPostComment(int postId, ApplicationUser author, Comment comment, int parentCommentId);

		Task AddPostTags(List<Tag> tags);

		Task AddPostLike(int postId, Like like);

		Task UpdatePost(int id, string title, string text, string imagePath);

		Task DeletePost(int id);

		Task DeletePostLike(int postId, int likeId);

		bool PostExists(int id);
	}
}
