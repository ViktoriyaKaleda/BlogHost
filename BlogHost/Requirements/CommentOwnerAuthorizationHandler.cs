using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace BlogHost.Requirements
{
	public class CommentOwnerAuthorizationHandler : AuthorizationHandler<OwnerRequirement, Comment>
	{
		private readonly IAccountService _accountService;

		public CommentOwnerAuthorizationHandler(IAccountService accountService)
		{
			_accountService = accountService;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerRequirement requirement, Comment resource)
		{
			var currentUser = await _accountService.GetCurrentUser(context.User);

			if (resource.Author.Id == currentUser.Id)
			{
				context.Succeed(requirement);
			}
		}
	}
}
