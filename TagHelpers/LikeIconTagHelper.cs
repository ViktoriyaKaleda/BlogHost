using System.Threading.Tasks;
using BlogH.Models;
using BlogHosting.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace BlogHosting.TagHelpers
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

		private readonly ApplicationDbContext _context;

		private readonly UserManager<IdentityUser> _userManager;

		public LikeIconTagHelper (ApplicationDbContext context,	UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "i";
			output.Attributes.SetAttribute("class", IconClass);
			output.Attributes.SetAttribute("id", IconId + PostId.ToString());

			IdentityUser user = await _userManager.GetUserAsync(ViewContext.HttpContext.User);
			Post post = await _context.Post.FirstOrDefaultAsync(m => m.PostId == PostId);
			if (user != null && post != null)
			{
				Like like = await _context.Like.FirstOrDefaultAsync(m => m.Owner == user && m.Post == post);
				if (like != null)
				{
					output.Attributes.SetAttribute("class", IconClass + " " + SelectedColor);
				}
			}
		}
	}
}
