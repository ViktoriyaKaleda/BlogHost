using DAL.Interface.DTO;
using DAL.Interface.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly BlogHostingDbContext _context;

		public UserRepository(UserManager<ApplicationUser> userManager,
			BlogHostingDbContext context, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_context = context;
			_signInManager = signInManager;
		}

		public async Task<IdentityResult> AddUser(ApplicationUser user, string password)
		{
			return await _userManager.CreateAsync(user, password);
		}

		public async Task DeleteUser(ApplicationUser user)
		{
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

		public List<ApplicationUser> GetAllUsers()
		{
			return _userManager.Users.ToList();
		}

		public async Task<ApplicationUser> GetUserById(string id)
		{
			return await _userManager.FindByIdAsync(id);
		}

		public async Task<ApplicationUser> GetUserByUsername(string username)
		{
			return await _userManager.FindByNameAsync(username);
		}

		public ApplicationUser GetUserByUsernamee(string username)
		{
			return _context.Users.FirstOrDefault(m => m.UserName == username);
		}

		public async Task<SignInResult> Login(string username, string password, bool rememberMe)
		{
			return await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);
		}

		public async Task Login(ApplicationUser user)
		{
			await _signInManager.SignInAsync(user, isPersistent: false);
		}

		public async Task Logout()
		{
			await _signInManager.SignOutAsync();
		}

		public async Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword)
		{
			var user = await GetUserById(userId);
			return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
		}

		public async Task<IdentityResult> UpdateEmailAsync(string userId, string newEmail)
		{
			var user = await GetUserById(userId);
			return await _userManager.SetEmailAsync(user, newEmail);
		}

		public async Task<IdentityResult> UpdatePhoneNumberAsync(string userId, string newPhoneNumber)
		{
			var user = await GetUserById(userId);
			return await _userManager.SetPhoneNumberAsync(user, newPhoneNumber);
		}

		public string GetUsername(ClaimsPrincipal principal)
		{
			return _userManager.GetUserName(principal);
		}

		public bool IsSignedIn(ClaimsPrincipal principal)
		{
			return _signInManager.IsSignedIn(principal);
		}

		public async Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal principal)
		{
			return await _userManager.GetUserAsync(principal);
		}

		public async Task UpdateFirstNameAsync(string userId, string firstName)
		{
			var user = await GetUserById(userId);
			user.FirstName = firstName;
			await _userManager.UpdateAsync(user);
		}

		public async Task UpdateLastNameAsync(string userId, string lastName)
		{
			var user = await GetUserById(userId);
			user.LastName = lastName;
			await _userManager.UpdateAsync(user);
		}

		public async Task UpdateAvatarAsync(string userId, string imagePath)
		{
			var user = await GetUserById(userId);
			user.AvatarPath = imagePath;
			await _userManager.UpdateAsync(user);
		}
	}
}
