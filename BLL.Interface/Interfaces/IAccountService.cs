using BLL.Interface.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BLL.Interface.Interfaces
{
	public interface IAccountService
	{
		Task<SignInResult> Login(string username, string passwors, bool rememberMe);

		Task Login(ApplicationUser user);

		Task<IdentityResult> Register(ApplicationUser user, string password);

		Task Logout();

		ApplicationUser GetUserByUsername(string username);
	}
}
