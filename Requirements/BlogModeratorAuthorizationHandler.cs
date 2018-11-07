using BlogH.Models;
using BlogHosting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlogHosting.Requirements
{
	public class BlogModeratorAuthorizationHandler : AuthorizationHandler<ModeratorRequirement, Blog>
	{
		UserManager<ApplicationUser> _userManager;

		public BlogModeratorAuthorizationHandler(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ModeratorRequirement requirement, Blog resource)
		{
			if (context.User == null || resource == null)
			{
				return Task.FromResult(0);
			}

			var currentUserId = _userManager.GetUserId(context.User);

			foreach (var blogModerator in resource.BlogModerators)
			{
				if (blogModerator.ModeratorId == currentUserId)
					context.Succeed(requirement);
			}

			return Task.FromResult(0);
		}
	}	
}
