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

		Task<ApplicationUser> GetUserById(string id);

		string GetUsername(ClaimsPrincipal principal);

		bool IsSignedIn(ClaimsPrincipal principal);

		Task<IdentityResult> UpdateEmailAsync(string userId, string email);

		Task<IdentityResult> UpdatePhoneNumberAsync(string userId, string phoneNumber);

		Task UpdateFirstNameAsync(string userId, string firstName);

		Task UpdateLastNameAsync(string userId, string lastName);

		Task UpdateAvatarAsync(string userId, string imagePath);

		Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
	}
}
