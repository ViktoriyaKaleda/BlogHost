using DAL.Interface.DTO;
using System.Threading.Tasks;

namespace DAL.Interface.Interfaces
{
	public interface IPostRepository
	{
		Task<Post> GetPostById(int id);

		void UpdatePost(Post post);

		void AddPost(Post post);

		void DeletePost(Post post);

		void AddPostTag(Post post, Tag tag);

		void AddPostLike(Post post, Like like);

		Task<Like> GetPostLike(Post post, ApplicationUser user);

		Task AddPostComment(Post post, Comment comment, int parentCommentId);
		
		Task Save();
	}
}
