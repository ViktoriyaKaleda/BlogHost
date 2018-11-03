using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogHosting.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRChat.Hubs;
using BlogHosting.Hubs;
using BlogHosting.Models;
using Microsoft.AspNetCore.Authorization;
using BlogHosting.Requirements;
using BlogHosting.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using BlogHosting.Configurations;

namespace BlogHosting
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
			{
				// Set your identity Settings here (password length, etc.)
			});

			services.AddTransient<IEmailSender, EmailSender>();

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

			services.AddSignalR();

			services.AddAuthorization(options =>
			{
				options.AddPolicy("OwnerPolicy", policy =>
					policy.Requirements.Add(new OwnerRequirement()));
			});

			services.AddScoped<IAuthorizationHandler,
						  BlogOwnerAuthorizationHandler>();

			services.AddScoped<IAuthorizationHandler,
						  PostOwnerAuthorizationHandler>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager)
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

			ApplicationDbInitializer.SeedUsers(userManager);

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			app.UseSignalR(routes =>
			{
				routes.MapHub<ChatHub>("/setLike");
			});

			app.UseSignalR(routes =>
			{
				routes.MapHub<CommentReplyHub>("/showReply");
			});
		}
	}
}
