using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace BlogHost.Requirements
{
	public class BlogModeratorAuthorizationHandler : AuthorizationHandler<ModeratorRequirement, Blog>
	{
		private readonly IBlogService _blogService;
		private readonly IAccountService _accountService;

		public BlogModeratorAuthorizationHandler(IBlogService blogService, IAccountService accountService)
		{
			_blogService = blogService;
			_accountService = accountService;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ModeratorRequirement requirement, Blog resource)
		{
			var currentUser = await _accountService.GetCurrentUser(context.User);

			var moderators = await _blogService.GetAllBlogModerators(resource.BlogId);

			foreach (var blogModerator in moderators)
			{
				if (blogModerator.Id == currentUser.Id)
					context.Succeed(requirement);
			}
		}
	}	
}
