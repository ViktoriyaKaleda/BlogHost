using System;
using System.Threading.Tasks;
using BLL.Interface.Interfaces;
using BlogHost.Models.ManageViewModels;
using BlogHost.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogHost.Controllers
{
	public class ManageController : Controller
    {
		private readonly IAccountService _accountService;
		private readonly IAuthenticateService _authenticateService;
		private readonly IImageService _imageService;
		private readonly ILogger _logger;

		public ManageController(
			IAccountService accountService,
			IAuthenticateService authenticateService,
			IImageService imageService,
			ILogger<AccountController> logger)
		{
			_accountService = accountService;
			_authenticateService = authenticateService;
			_imageService = imageService;
			_logger = logger;
		}

		[TempData]
		public string StatusMessage { get; set; }

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var user = await _accountService.GetCurrentUser(User);

			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with Name '{User.Identity.Name}'.");
			}

			var model = new IndexViewModel
			{
				Username = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				AvatarPath = user.AvatarPath,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				IsEmailConfirmed = user.EmailConfirmed,
				StatusMessage = StatusMessage
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(IndexViewModel model, string username = null)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _accountService.GetCurrentUser(User);

			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with Name '{User.Identity.Name}'.");
			}

			var email = user.Email;
			if (model.Email != email)
			{
				var setEmailResult = await _accountService.UpdateEmailAsync(user, model.Email);
				if (!setEmailResult.Succeeded)
				{
					throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
				}
			}

			var phoneNumber = user.PhoneNumber;
			if (model.PhoneNumber != phoneNumber)
			{
				var setPhoneResult = await _accountService.UpdatePhoneNumberAsync(user, model.PhoneNumber);
				if (!setPhoneResult.Succeeded)
				{
					throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
				}
			}

			if (model.AvatarFile?.FileName != null)
			{
				user.AvatarPath = await _imageService.SaveAvatarImage(model.AvatarFile, user.AvatarPath);
			}

			if (model.FirstName != user.FirstName)
			{
				await _accountService.UpdateFirstNameAsync(user, model.FirstName);
			}

			if (model.LastName != user.LastName)
			{
				await _accountService.UpdateLastNameAsync(user, model.LastName);
			}
			
			StatusMessage = "Profile has been updated";

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public IActionResult ChangePassword()
		{
			var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _accountService.GetCurrentUser(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with Name '{User.Identity.Name}'.");
			}

			var changePasswordResult = await _accountService.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
			if (!changePasswordResult.Succeeded)
			{
				AddErrors(changePasswordResult);
				return View(model);
			}

			await _authenticateService.Login(user.UserName, model.NewPassword, false);
			_logger.LogInformation("User changed their password successfully.");
			StatusMessage = "Your password has been changed.";

			return RedirectToAction(nameof(ChangePassword));
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