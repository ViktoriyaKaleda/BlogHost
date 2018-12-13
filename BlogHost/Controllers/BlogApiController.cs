using System;
using System.Threading.Tasks;
using BLL.Interface.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogHost.Controllers
{
	public class BlogApiController : Controller
    {
		private readonly IBlogService _blogService;
		private readonly IAccountService _accountService;

		public BlogApiController(
			IBlogService blogService,
			IAccountService accountService)
		{
			_blogService = blogService;
			_accountService = accountService;
		}

		[HttpPost("Blog/Edit/{id}/AddModerator")]
		public async Task<IActionResult> AddModerator(int id, [FromBody] string text)
		{
			if (String.IsNullOrEmpty(text))
				return BadRequest("Field can not be empty.");

			var user = await _accountService.GetUserByUsername(text);

			if (user == null)
				return NotFound("User is not found.");

			if (user == await _accountService.GetCurrentUser(HttpContext.User))
				return BadRequest("You already have absolute rights for this blog.");

			if (!_blogService.BlogExists(id))
				return NotFound();

			if (await _blogService.GetBlogModeratorById(id, user.Id) != null)
				return BadRequest("This user is already moderator.");

			await _blogService.AddBlogModerator(id, user);

			return PartialView("~/Views/Blog/_ModeratorsPartial.cshtml", await _blogService.GetAllBlogModerators(id));
		}

		[HttpPost("Blog/Edit/{id}/DeleteModerator")]
		public async Task<IActionResult> DeleteModerator(int id, [FromBody] string text)
		{
			if (String.IsNullOrEmpty(text))
				return BadRequest("Field can not be empty.");

			var user = await _accountService.GetUserByUsername(text);

			if (user == null)
				return NotFound("User is not found.");

			if (!_blogService.BlogExists(id))
				return NotFound();

			var oldModerator = await _blogService.GetBlogModeratorById(id, user.Id);

			await _blogService.DeleteBlogModerator(id, user);

			var temp = await _blogService.GetAllBlogModerators(id);

			return PartialView("~/Views/Blog/_ModeratorsPartial.cshtml", temp);
		}
	}
}