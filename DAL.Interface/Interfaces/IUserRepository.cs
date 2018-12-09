using DAL.Interface.DTO;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DAL.Interface.Interfaces
{
	public interface IUserRepository
	{
		Task<ApplicationUser> GetUserById(string id);

		Task<ApplicationUser> GetUserByUsername(string username);

		Task<IdentityResult> AddUser(ApplicationUser user, string password);

		Task UpdateUser(ApplicationUser user);

		Task DeleteUser(string id);
	}
}
