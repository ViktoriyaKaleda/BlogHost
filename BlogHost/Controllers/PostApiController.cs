using System.Linq;
using System.Threading.Tasks;
using BLL.Interface.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogHost.Controllers
{
	public class PostApiController : Controller
    {
		private readonly IPostService _postService;
		private readonly IAuthorizationService _authorizationService;

		public PostApiController(
			IPostService postService,
			IAuthorizationService authorizationService)
		{
			_postService = postService;
			_authorizationService = authorizationService;
		}

		[HttpPost("Post/Details/{id}/DeleteComment")]
		[Authorize]
		public async Task<IActionResult> DeleteComment(int id, [FromBody]int? commentId)
		{
			var comment = await _postService.GetPostComment((int)commentId);
			var postId = comment.Post.PostId;

			if (comment == null)
				return NotFound();

			if (!(await _authorizationService.AuthorizeAsync(User, comment, "OwnerPolicy")).Succeeded
				&& !(await _authorizationService.AuthorizeAsync(User, comment.Post.Blog, "ModeratorPolicy")).Succeeded)
				return Forbid();

			await _postService.DeletePostComment((int)commentId);

			return PartialView("~/Views/Post/_CommentPartial.cshtml",
				(await _postService.GetPostById(postId)).Comments.Where(m => m.ParentCommentId == 0).ToList());
		}
	}
}