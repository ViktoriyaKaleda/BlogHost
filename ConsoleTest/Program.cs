using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using BLL.Services;
using DAL;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ConsoleTest
{
	class Program
	{
		private static IServiceProvider serviceProvider;

		private static void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<BlogHostingDbContext>(options =>
				options.UseLazyLoadingProxies()
					.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-BlogHosting-123BCBEF-17A2-4F0F-8B69-F13F4F2ADE44;Trusted_Connection=True;MultipleActiveResultSets=true"));

			// Register UserManager & RoleManager
			services.AddIdentity<DAL.Interface.DTO.ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<BlogHostingDbContext>()
				.AddDefaultTokenProviders();

			// UserManager & RoleManager require logging and HttpContext dependencies
			services.AddLogging();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddScoped<IAuthenticateService, AuthenticateService>();

			services.AddScoped<SignInManager<DAL.Interface.DTO.ApplicationUser>, SignInManager<DAL.Interface.DTO.ApplicationUser>>();
		}

		static void Main(string[] args)
		{
			var services = new ServiceCollection();
			ConfigureServices(services);
			serviceProvider = services.BuildServiceProvider();

			var optionsBuilder = new DbContextOptionsBuilder<BlogHostingDbContext>();
			optionsBuilder
				.UseLazyLoadingProxies()
				.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-BlogHosting-123BCBEF-17A2-4F0F-8B69-F13F4F2ADE44;Trusted_Connection=True;MultipleActiveResultSets=true");

			//var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var signInManager = serviceProvider.GetRequiredService<SignInManager<DAL.Interface.DTO.ApplicationUser>>();

			using (var context = new BlogHostingDbContext(optionsBuilder.Options))
			{
				var rep = new UserRepository(serviceProvider.GetRequiredService<UserManager<DAL.Interface.DTO.ApplicationUser>>()
					, context, signInManager);

				var service = new AuthenticateService(rep);

				//var user =  service.GetUserByUsernamee("User1");

				//Console.WriteLine(user.FirstName);
			}
				
		}		 
	}
}
