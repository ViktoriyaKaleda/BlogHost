using BLL.Interface.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace BlogHost.TagHelpers
{
	public class LikeIconTagHelper : TagHelper
	{
		private string IconClass { get; set; } = "fas fa-heart";

		private string IconId { get; set; } = "likeIcon";

		private string SelectedColor { get; set; } = "red";

		public int PostId { get; set; }

		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext ViewContext { get; set; }

		private readonly IPostService _postService;

		private readonly IAccountService _accountService;

		public LikeIconTagHelper(IPostService postService, IAccountService accountService)
		{
			_postService = postService;
			_accountService = accountService;
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "i";
			output.Attributes.SetAttribute("class", IconClass);
			output.Attributes.SetAttribute("id", IconId + PostId.ToString());

			var user = await _accountService.GetCurrentUser(ViewContext.HttpContext.User);
			var post = await _postService.GetPostById(PostId);
			if (user != null && post != null)
			{
				var like = await _postService.GetPostLike(post, user);
				if (like != null)
				{
					output.Attributes.SetAttribute("class", IconClass + " " + SelectedColor);
				}
			}
		}
	}
}
