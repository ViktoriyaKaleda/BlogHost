using DAL.Interface.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interface.Interfaces
{
	public interface IPostRepository
	{
		Task<Post> GetPostById(int id);

		List<Post> GetAllPosts();

		Task UpdatePost(int postId, string title, string text, string imagePath);

		Task AddPost(Post post);

		Task DeletePost(int postId);

		Task AddPostTag(int postId, Tag tag);

		Task AddPostTags(List<Tag> tags);

		Task AddPostLike(int postId, Like like);

		Task DeletePostLike(int postId, int likeId);

		Task<Like> GetPostLike(Post post, ApplicationUser user);

		Task AddPostComment(int postId, ApplicationUser author, Comment comment, int parentCommentId);

		bool PostExists(int id);
	}
}
