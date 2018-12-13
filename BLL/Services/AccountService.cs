using AutoMapper;
using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using BLL.Mappers;
using DAL.Interface.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class AccountService : IAccountService
	{
		private readonly IUserRepository _repository;

		public AccountService(IUserRepository repository)
		{
			_repository = repository;			
		}

		public async Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
		{
			return await _repository.ChangePassword(userId, oldPassword, newPassword);
		}

		public async Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal principal)
		{
			return Mapper.Map<ApplicationUser>(await _repository.GetCurrentUser(principal));
		}

		public async Task UpdateAvatarAsync(string userId, string imagePath)
		{
			await _repository.UpdateAvatarAsync(userId, imagePath);
		}

		public async Task<IdentityResult> UpdateEmailAsync(string userId, string email)
		{
			return await _repository.UpdateEmailAsync(userId, email);
		}

		public async Task UpdateFirstNameAsync(string userId, string firstName)
		{
			await _repository.UpdateFirstNameAsync(userId, firstName);
		}

		public async Task UpdateLastNameAsync(string userId, string lastName)
		{
			await _repository.UpdateLastNameAsync(userId, lastName);
		}

		public async Task<IdentityResult> UpdatePhoneNumberAsync(string userId, string phoneNumber)
		{
			return await _repository.UpdatePhoneNumberAsync(userId, phoneNumber);
		}

		public async Task<ApplicationUser> GetUserByUsername(string username)
		{
			return Mapper.Map<ApplicationUser>(await _repository.GetUserByUsername(username));
		}

		public ApplicationUser GetUserByUsernamee(string username)
		{
			return Mapper.Map<ApplicationUser>(_repository.GetUserByUsernamee(username));
		}

		public string GetUsername(ClaimsPrincipal principal)
		{
			return _repository.GetUsername(principal);
		}

		public bool IsSignedIn(ClaimsPrincipal principal)
		{
			return _repository.IsSignedIn(principal);
		}
	}
}
