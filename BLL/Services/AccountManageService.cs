using AutoMapper;
using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using BLL.Mappers;
using DAL.Interface.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class AccountManageService : IAccountManageService
	{
		private readonly IUserRepository _repository;

		public AccountManageService(IUserRepository repository)
		{
			_repository = repository;			
		}

		public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string newPassword)
		{
			return await _repository.ChangePassword(Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user), oldPassword, newPassword);
		}

		public async Task UpdateAvatarAsync(ApplicationUser user, string imagePath)
		{
			user.AvatarPath = imagePath;
			await _repository.UpdateUser(Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user));
		}

		public async Task<IdentityResult> UpdateEmailAsync(ApplicationUser user, string email)
		{
			return await _repository.UpdateEmailAsync(Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user), email);
		}

		public async Task UpdateFirstNameAsync(ApplicationUser user, string firstName)
		{
			user.FirstName = firstName;
			await _repository.UpdateUser(Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user));
		}

		public async Task UpdateLastNameAsync(ApplicationUser user, string lastName)
		{
			user.LastName = lastName;
			await _repository.UpdateUser(Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user));
		}

		public async Task<IdentityResult> UpdatePhoneNumberAsync(ApplicationUser user, string phoneNumber)
		{
			return await _repository.UpdatePhoneNumberAsync(Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user), phoneNumber);
		}
	}
}
