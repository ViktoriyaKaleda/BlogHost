using DAL.Interface.DTO;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DAL.Interface.Interfaces
{
	public interface IUserRepository
	{
		Task<ApplicationUser> GetUserById(string id);

		Task<ApplicationUser> GetUserByUsername(string username);

		Task<IdentityResult> AddUser(ApplicationUser user, string password);

		Task UpdateFirstNameAsync(string userId, string firstName);

		Task UpdateLastNameAsync(string userId, string lastName);

		Task<IdentityResult> UpdateEmailAsync(string userId, string newEmail);

		Task<IdentityResult> UpdatePhoneNumberAsync(string userId, string newPhoneNumber);

		Task UpdateAvatarAsync(string userId, string imagePath);

		Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword);

		Task DeleteUser(ApplicationUser user);

		Task<SignInResult> Login(string username, string passwors, bool rememberMe);

		Task Login(ApplicationUser user);

		Task Logout();

		Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal principal);

		string GetUsername(ClaimsPrincipal principal);

		bool IsSignedIn(ClaimsPrincipal principal);

		//delete later
		ApplicationUser GetUserByUsernamee(string username);
	}
}
