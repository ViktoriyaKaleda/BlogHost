using AutoMapper;
using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using BLL.Mappers;
using DAL.Interface.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class AccountService : IAccountService
	{
		private readonly IUserRepository _repository;

		public AccountService( IUserRepository repository)
		{
			_repository = repository;
			Mapper.Initialize(c => c.AddProfile<MappingProfile>());
		}

		public async Task<ApplicationUser> GetUserByUsername(string username)
		{ 
			return Mapper.Map<ApplicationUser>(await _repository.GetUserByUsername(username));
		}

		public ApplicationUser GetUserByUsernamee(string username)
		{
			return Mapper.Map<ApplicationUser>(_repository.GetUserByUsernamee(username));
		}

		public async Task<SignInResult> Login(string username, string password, bool rememberMe)
		{
			return await _repository.Login(username, password, rememberMe);
		}

		public async Task Login(ApplicationUser user)
		{
			await _repository.Login(Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user));
		}

		public async Task Logout()
		{
			await _repository.Logout();
		}
	}
}
