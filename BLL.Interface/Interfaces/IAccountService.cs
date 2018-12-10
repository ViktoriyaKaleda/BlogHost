using BLL.Interface.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BLL.Interface.Interfaces
{
	public interface IAccountService
	{
		Task<SignInResult> Login(string username, string passwors, bool rememberMe);

		Task Login(ApplicationUser user);

		Task Logout();

		Task<ApplicationUser> GetUserByUsername(string username);

		ApplicationUser GetUserByUsernamee(string username);
	}
}
