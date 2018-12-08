using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogHosting.Data;
using BlogHosting.Models;
using BlogHosting.Models.AccountViewModels;
using BlogHosting.Models.PageNavigationViewModels;
using BlogHosting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogHosting.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ManageController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IEmailSender _emailSender;
		private readonly ILogger<ManageController> _logger;
		private readonly IImageService _imageService;

		public ManageController(UserManager<ApplicationUser> userManager,
			ApplicationDbContext context,
			IEmailSender emailSender,
			ILogger<ManageController> logger,
			IImageService imageService
			)
		{
			_userManager = userManager;
			_context = context;
			_emailSender = emailSender;
			_logger = logger;
			_imageService = imageService;
		}

		[Authorize(Roles = "Admin")]
		public IActionResult Index()
        {
            return View();
        }

		[Authorize(Roles = "Admin")]
		[Route("[area]/[controller]/Users")]
		public async Task<IActionResult> GetAllUsers(int page = 1)
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);
			var roles = await _userManager.GetRolesAsync(user);

			int pageSize = 3;   // number of users on page

			IQueryable<ApplicationUser> source = _context.Users;
			var count = await source.CountAsync();
			var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
			UsersPageViewModel viewModel = new UsersPageViewModel
			{
				PageViewModel = pageViewModel,
				Users = items
			};

			return View(viewModel);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		[Route("[area]/[controller]/Users/Create")]
		public IActionResult CreateUser()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("[area]/[controller]/Users/Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateUser(RegisterViewModel model, string returnUrl = null)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					UserName = model.Username,
					FirstName = model.FirstName,
					LastName = model.LastName,
					Email = model.Email
				};

				if (model.AvatarFile?.FileName != null)
				{
					user.AvatarPath = await _imageService.SaveAvatarImage(model.AvatarFile);
				}

				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					_logger.LogInformation("Admin created a new user account with password.");

					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
					await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
					return RedirectToAction(nameof(GetAllUsers));
				}
				AddErrors(result);
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		#region Helpers

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}
		#endregion
	}
}