using BLL.Interface.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interface.Interfaces
{
	public interface IPostService
	{
		Task<Post> GetPostById(int id);

		Task<List<Post>> GetAllPosts();

		Task AddPost(Post post, IFormFile image);

		Task AddPostComment(int postId, ApplicationUser author, Comment comment, string parentCommentId);

		Task UpdatePost(int id, string title, string text, IFormFile image, List<Tag> tags);

		Task DeletePost(int id);
	}
}
