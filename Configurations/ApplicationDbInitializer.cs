using BlogHosting.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogHosting.Configurations
{
	public static class ApplicationDbInitializer
	{
		public static void SeedUsers(UserManager<ApplicationUser> userManager)
		{
			if (userManager.FindByEmailAsync("adminmail@mail.com").Result == null)
			{
				ApplicationUser user = new ApplicationUser
				{
					UserName = "admin",
					Email = "adminmail@mail.com"
				};

				IdentityResult result = userManager.CreateAsync(user, "abcD1_").Result;

				if (result.Succeeded)
				{
					userManager.AddToRoleAsync(user, "Admin").Wait();
				}
			}
		}
	}
}
