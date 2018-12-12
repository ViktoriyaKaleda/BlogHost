using BLL.Interface.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BLL.Interface.Interfaces
{
	public interface IAuthenticateService
	{
		Task<IdentityResult> CreateUser(ApplicationUser user, string password);

		Task<SignInResult> Login(string username, string passwors, bool rememberMe);

		Task Logout();
	}
}
