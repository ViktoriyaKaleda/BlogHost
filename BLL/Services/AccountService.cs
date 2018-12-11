using AutoMapper;
using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using BLL.Mappers;
using DAL.Interface.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class AccountService : IAccountService
	{
		private readonly IUserRepository _repository;

		public AccountService( IUserRepository repository)
		{
			_repository = repository;
		}

		public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
		{
			return await _repository.AddUser(Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user), password);
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
