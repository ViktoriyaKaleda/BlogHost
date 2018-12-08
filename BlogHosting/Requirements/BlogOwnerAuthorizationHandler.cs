using BlogH.Models;
using BlogHosting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlogHosting.Requirements
{
	public class BlogOwnerAuthorizationHandler : AuthorizationHandler<OwnerRequirement, Blog>
	{
		UserManager<ApplicationUser> _userManager;

		public BlogOwnerAuthorizationHandler(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerRequirement requirement, Blog resource)
		{
			if (context.User == null || resource == null)
			{
				return Task.FromResult(0);
			}

			if (resource.Author.Id == _userManager.GetUserId(context.User))
			{
				context.Succeed(requirement);
			}

			return Task.FromResult(0);
		}
	}	
}
