using DAL.Interface.DTO;
using System.Threading.Tasks;

namespace DAL.Interface.Interfaces
{
	public interface IPostRepository
	{
		Task<Post> GetPostById(int id);

		void UpdatePost(Post post);

		void AddPost(Post post);

		void DeletePost(int id);

		void AddPostTag(Post post, Tag tag);

		void AddPostLike(Post post, Like like);

		void AddPostComment(Post post, Comment comment, Comment parentComment);
		
		Task Save();
	}
}
