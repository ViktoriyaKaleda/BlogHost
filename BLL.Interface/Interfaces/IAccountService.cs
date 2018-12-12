using BLL.Interface.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interface.Interfaces
{
	public interface IAccountService
	{
		Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal principal);

		Task<ApplicationUser> GetUserByUsername(string username);

		string GetUsername(ClaimsPrincipal principal);

		bool IsSignedIn(ClaimsPrincipal principal);

		Task<IdentityResult> UpdateEmailAsync(ApplicationUser user, string email);

		Task<IdentityResult> UpdatePhoneNumberAsync(ApplicationUser user, string phoneNumber);

		Task UpdateFirstNameAsync(ApplicationUser user, string firstName);

		Task UpdateLastNameAsync(ApplicationUser user, string lastName);

		Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword);
	}
}
