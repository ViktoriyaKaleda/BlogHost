using DAL;
using DAL.Interface.DTO;
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
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<BlogHostingDbContext>()
				.AddDefaultTokenProviders();

			// UserManager & RoleManager require logging and HttpContext dependencies
			services.AddLogging();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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

			var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();


			using (var context = new BlogHostingDbContext(optionsBuilder.Options))
			{
				var rep = new UserRepository(serviceProvider.GetRequiredService<UserManager<ApplicationUser>>(), context);

				var users =  rep.GetAllUsers();

				foreach (var user in users)
				{
					Console.WriteLine(user.UserName);
				}
			}
				
		}		
	}
}
