using DAL.Interface.DTO;
using System.Threading.Tasks;

namespace DAL.Interface.Interfaces
{
	public interface IUserRepository
	{
		Task<ApplicationUser> GetUserById(string id);

		Task<ApplicationUser> GetUserByUsername(string username);

		void AddUser(ApplicationUser user);

		void UpdateUser(ApplicationUser user);

		void DeleteUser(ApplicationUser user);

		Task Save();
	}
}
