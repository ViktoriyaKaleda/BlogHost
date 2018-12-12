using System.Globalization;
using AutoMapper;
using BLL.Interface.Interfaces;
using BLL.Mappers;
using BLL.Services;
using BlogHost.Requirements;
using BlogHost.Services;
using DAL.Interface.DTO;
using DAL.Interface.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlogHost
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDbContext<DAL.BlogHostingDbContext>(options =>
				options.UseLazyLoadingProxies()
					.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<DAL.BlogHostingDbContext>()
				.AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
			{
				// Set your identity Settings here (password length, etc.)
			});

			services.AddLocalization(options => options.ResourcesPath = "Resources");
					   
			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
				.AddDataAnnotationsLocalization()
				.AddViewLocalization();

			services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new[]
				{
					new CultureInfo("en"),
					new CultureInfo("de"),
					new CultureInfo("ru")
				};

				options.DefaultRequestCulture = new RequestCulture("ru");
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
			});

			services.AddMvcCore().AddJsonFormatters();

			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = $"/account/login";
				options.LogoutPath = $"/account/logout";
				options.AccessDeniedPath = $"/account/access-denied";
			});

			services.AddSignalR(o =>
			{
				o.EnableDetailedErrors = true;
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("OwnerPolicy", policy =>
					policy.Requirements.Add(new OwnerRequirement()));

				options.AddPolicy("ModeratorPolicy", policy =>
					policy.Requirements.Add(new ModeratorRequirement()));
			});

			services.AddScoped<IAuthorizationHandler,
						  BlogOwnerAuthorizationHandler>();

			services.AddScoped<IAuthorizationHandler,
						  PostOwnerAuthorizationHandler>();

			services.AddScoped<IAuthorizationHandler,
						  CommentOwnerAuthorizationHandler>();

			services.AddScoped<IAuthorizationHandler,
						  BlogModeratorAuthorizationHandler>();

			MapperInitializer.MapperConfiguration();

			services.AddScoped<IUserRepository, UserRepository>();

			services.AddScoped<IBlogRepository, BlogRepository>();

			services.AddScoped<IPostRepository, PostRepository>();

			services.AddScoped<IAuthenticateService, AuthenticateService>();

			services.AddScoped<IAccountService, AccountService>();

			services.AddScoped<IBlogService, BlogService>();

			services.AddScoped<IPostService, PostService>();

			services.AddSingleton<IImageService, ImageService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
			app.UseRequestLocalization(locOptions.Value);

			app.UseStaticFiles();

			app.UseCookiePolicy();

			app.UseAuthentication();

			//using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			//{
			//	var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
			//	DAL.Configurations.ApplicationDbInitializer.SeedUsers(userManager);
			//}				

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
