using BLL.Interface.Entities;
using BLL.Interface.Interfaces;
using BlogHost.Models.AccountViewModels;
using BlogHost.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BlogHost.Controllers
{
	public class AccountController : Controller
    {
		private readonly IAccountService _accountService;
		private readonly IImageService _imageService;
		private readonly ILogger _logger;

		public AccountController(
			IAccountService accountService,
			IImageService imageService,
			ILogger<AccountController> logger)
		{
			_accountService = accountService;
			_imageService = imageService;
			_logger = logger;
		}

		[TempData]
		public string ErrorMessage { get; set; }

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Login(string returnUrl = null)
		{
			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			if (ModelState.IsValid)
			{
				var result = await _accountService.Login(model.Username, model.Password, model.RememberMe);
				if (result.Succeeded)
				{
					_logger.LogInformation("User logged in.");
					return RedirectToLocal(returnUrl);
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return View(model);
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Register(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
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

				var result = await _accountService.CreateUser(user, model.Password);
				if (result.Succeeded)
				{
					await _accountService.Login(user.UserName, model.Password, false);
					_logger.LogInformation("User created a new account with password.");
					return RedirectToLocal(returnUrl);
				}
				AddErrors(result);
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _accountService.Logout();
			_logger.LogInformation("User logged out.");
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		#region Helpers

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		private IActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction(nameof(HomeController.Index), "Home");
			}
		}

		#endregion
	}
}