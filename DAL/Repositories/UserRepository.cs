using DAL.Interface.DTO;
using DAL.Interface.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly BlogHostingDbContext _context;

		public UserRepository(UserManager<ApplicationUser> userManager, BlogHostingDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		public async Task<IdentityResult> AddUser(ApplicationUser user, string password)
		{
			return await _userManager.CreateAsync(user, password);
		}

		public async Task DeleteUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			var blogModeratros = await _context.BlogModerator.Where(m => m.ModeratorId == user.Id).ToListAsync();

			if (blogModeratros.Count() != 0)
			{
				foreach (var blogModerator in blogModeratros)
				{
					_context.BlogModerator.Remove(blogModerator);
				}
				await _context.SaveChangesAsync();
			}

			_context.UserLogins.RemoveRange(_context.UserLogins.Where(ul => ul.UserId == user.Id));

			_context.UserRoles.RemoveRange(_context.UserRoles.Where(ur => ur.UserId == user.Id));

			_context.Users.Remove(_context.Users.Where(usr => usr.Id == user.Id).Single());

			await _context.SaveChangesAsync();
		}

		public async Task<ApplicationUser> GetUserById(string id)
		{
			return await _userManager.FindByIdAsync(id);
		}

		public async Task<ApplicationUser> GetUserByUsername(string username)
		{
			return await _userManager.FindByNameAsync(username);
		}

		public async Task UpdateUser(ApplicationUser user)
		{
			await _userManager.UpdateAsync(user);
		}
	}
}
