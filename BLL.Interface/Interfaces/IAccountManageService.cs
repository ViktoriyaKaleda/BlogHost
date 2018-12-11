using BLL.Interface.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BLL.Interface.Interfaces
{
	public interface IAccountManageService
	{
		Task<IdentityResult> UpdateEmailAsync(ApplicationUser user, string email);

		Task<IdentityResult> UpdatePhoneNumberAsync(ApplicationUser user, string phoneNumber);

		Task UpdateFirstNameAsync(ApplicationUser user, string firstName);

		Task UpdateLastNameAsync(ApplicationUser user, string lastName);

		Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword);
	}
}
