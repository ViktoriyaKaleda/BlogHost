using AutoMapper;
using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using DAL.Interface.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class AuthenticateService : IAuthenticateService
	{
		private readonly IUserRepository _repository;

		public AuthenticateService( IUserRepository repository)
		{
			_repository = repository;
		}

		public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
		{
			return await _repository.AddUser(Mapper.Map<DAL.Interface.DTO.ApplicationUser>(user), password);
		}		

		public async Task<SignInResult> Login(string username, string password, bool rememberMe)
		{
			return await _repository.Login(username, password, rememberMe);
		}

		public async Task Logout()
		{
			await _repository.Logout();
		}
	}
}
