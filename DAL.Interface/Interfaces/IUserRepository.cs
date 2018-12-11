﻿using DAL.Interface.DTO;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interface.Interfaces
{
	public interface IUserRepository
	{
		Task<ApplicationUser> GetUserById(string id);

		Task<ApplicationUser> GetUserByUsername(string username);

		Task<IdentityResult> AddUser(ApplicationUser user, string password);

		Task UpdateUser(ApplicationUser user);

		Task<IdentityResult> UpdateEmailAsync(ApplicationUser user, string newEmail);

		Task<IdentityResult> UpdatePhoneNumberAsync(ApplicationUser user, string newPhoneNumber);

		Task<IdentityResult> ChangePassword(ApplicationUser user,string oldPassword, string newPassword);

		Task DeleteUser(ApplicationUser user);

		Task<SignInResult> Login(string username, string passwors, bool rememberMe);

		Task Login(ApplicationUser user);

		Task Logout();

		//delete later
		ApplicationUser GetUserByUsernamee(string username);
	}
}
