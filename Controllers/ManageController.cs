using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BlogHosting.Data;
using BlogHosting.Models;
using BlogHosting.Models.AccountViewModels;
using BlogHosting.Models.ManageViewModels;
using BlogHosting.Models.PageNavigationViewModels;
using BlogHosting.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogHosting.Controllers
{
	[Authorize]
	[Route("[controller]/[action]")]
	public class ManageController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly ILogger _logger;
		private readonly UrlEncoder _urlEncoder;
		private readonly IHostingEnvironment _appEnvironment;
		private readonly ApplicationDbContext _context;

		private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
		private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

		public ManageController(
		  UserManager<ApplicationUser> userManager,
		  SignInManager<ApplicationUser> signInManager,
		  IEmailSender emailSender,
		  ILogger<ManageController> logger,
		  UrlEncoder urlEncoder,
		  IHostingEnvironment appEnvironment,
		  ApplicationDbContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_logger = logger;
			_urlEncoder = urlEncoder;
			_appEnvironment = appEnvironment;
			_context = context;
		}

		[TempData]
		public string StatusMessage { get; set; }

		[HttpGet]
		public async Task<IActionResult> Index(string username = null)
		{
			ApplicationUser user;

			if (username == null)
				user = await _userManager.GetUserAsync(User);
			else
				user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == username);

			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var model = new IndexViewModel
			{
				Username = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				AvatarPath =  user.AvatarPath,
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

			ApplicationUser user;

			if (username == null)
				user = await _userManager.GetUserAsync(User);
			else
				user = await _context.Users.FirstOrDefaultAsync(m => m.UserName == username);

			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var email = user.Email;
			if (model.Email != email)
			{
				var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
				if (!setEmailResult.Succeeded)
				{
					throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
				}
			}

			var phoneNumber = user.PhoneNumber;
			if (model.PhoneNumber != phoneNumber)
			{
				var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
				if (!setPhoneResult.Succeeded)
				{
					throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
				}
			}

			if (model.AvatarFile?.FileName != null)
			{
				if (user.AvatarPath != null)
				{
					try
					{
						System.IO.File.Delete(_appEnvironment.WebRootPath + "/Avatars/" + Path.GetFileName(user.AvatarPath));
					}
					catch (System.IO.IOException e)
					{
						_logger.LogWarning("Failed to delete user avatar file. File path: {}", user.AvatarPath);
					}
				}

				string path = GetAvatarPath(model.AvatarFile);

				user.AvatarPath = "~/" + path;

				using (var fileStream = new FileStream(_appEnvironment.WebRootPath + "/" + path, FileMode.Create))
				{
					await model.AvatarFile.CopyToAsync(fileStream);
				}

				await _userManager.UpdateAsync(user);
			}

			if (model.FirstName != user.FirstName)
			{
				user.FirstName = model.FirstName;
				await _userManager.UpdateAsync(user);
			}

			if (model.LastName != user.LastName)
			{
				user.LastName = model.LastName;
				await _userManager.UpdateAsync(user);
			}

			StatusMessage = "Profile has been updated";

			if (username != null)
				return RedirectToAction(nameof(GetAllUsers));

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
			var email = user.Email;
			await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);

			StatusMessage = "Verification email sent. Please check your email.";
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> ChangePassword()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var hasPassword = await _userManager.HasPasswordAsync(user);
			if (!hasPassword)
			{
				return RedirectToAction(nameof(SetPassword));
			}

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

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
			if (!changePasswordResult.Succeeded)
			{
				AddErrors(changePasswordResult);
				return View(model);
			}

			await _signInManager.SignInAsync(user, isPersistent: false);
			_logger.LogInformation("User changed their password successfully.");
			StatusMessage = "Your password has been changed.";

			return RedirectToAction(nameof(ChangePassword));
		}

		[HttpGet]
		public async Task<IActionResult> SetPassword()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var hasPassword = await _userManager.HasPasswordAsync(user);

			if (hasPassword)
			{
				return RedirectToAction(nameof(ChangePassword));
			}

			var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
			if (!addPasswordResult.Succeeded)
			{
				AddErrors(addPasswordResult);
				return View(model);
			}

			await _signInManager.SignInAsync(user, isPersistent: false);
			StatusMessage = "Your password has been set.";

			return RedirectToAction(nameof(SetPassword));
		}

		[HttpGet]
		public async Task<IActionResult> ExternalLogins()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var model = new ExternalLoginsViewModel { CurrentLogins = await _userManager.GetLoginsAsync(user) };
			model.OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
				.Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
				.ToList();
			model.ShowRemoveButton = await _userManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;
			model.StatusMessage = StatusMessage;

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LinkLogin(string provider)
		{
			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			// Request a redirect to the external login provider to link a login for the current user
			var redirectUrl = Url.Action(nameof(LinkLoginCallback));
			var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
			return new ChallengeResult(provider, properties);
		}

		[HttpGet]
		public async Task<IActionResult> LinkLoginCallback()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var info = await _signInManager.GetExternalLoginInfoAsync(user.Id);
			if (info == null)
			{
				throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
			}

			var result = await _userManager.AddLoginAsync(user, info);
			if (!result.Succeeded)
			{
				throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
			}

			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			StatusMessage = "The external login was added.";
			return RedirectToAction(nameof(ExternalLogins));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var result = await _userManager.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
			if (!result.Succeeded)
			{
				throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
			}

			await _signInManager.SignInAsync(user, isPersistent: false);
			StatusMessage = "The external login was removed.";
			return RedirectToAction(nameof(ExternalLogins));
		}

		[HttpGet]
		public async Task<IActionResult> TwoFactorAuthentication()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var model = new TwoFactorAuthenticationViewModel
			{
				HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
				Is2faEnabled = user.TwoFactorEnabled,
				RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Disable2faWarning()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!user.TwoFactorEnabled)
			{
				throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
			}

			return View(nameof(Disable2fa));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Disable2fa()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
			if (!disable2faResult.Succeeded)
			{
				throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
			}

			_logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
			return RedirectToAction(nameof(TwoFactorAuthentication));
		}

		[HttpGet]
		public async Task<IActionResult> EnableAuthenticator()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var model = new EnableAuthenticatorViewModel();
			await LoadSharedKeyAndQrCodeUriAsync(user, model);

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!ModelState.IsValid)
			{
				await LoadSharedKeyAndQrCodeUriAsync(user, model);
				return View(model);
			}

			// Strip spaces and hypens
			var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

			var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
				user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

			if (!is2faTokenValid)
			{
				ModelState.AddModelError("Code", "Verification code is invalid.");
				await LoadSharedKeyAndQrCodeUriAsync(user, model);
				return View(model);
			}

			await _userManager.SetTwoFactorEnabledAsync(user, true);
			_logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
			var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
			TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

			return RedirectToAction(nameof(ShowRecoveryCodes));
		}

		[HttpGet]
		public IActionResult ShowRecoveryCodes()
		{
			var recoveryCodes = (string[])TempData[RecoveryCodesKey];
			if (recoveryCodes == null)
			{
				return RedirectToAction(nameof(TwoFactorAuthentication));
			}

			var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes };
			return View(model);
		}

		[HttpGet]
		public IActionResult ResetAuthenticatorWarning()
		{
			return View(nameof(ResetAuthenticator));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetAuthenticator()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			await _userManager.SetTwoFactorEnabledAsync(user, false);
			await _userManager.ResetAuthenticatorKeyAsync(user);
			_logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

			return RedirectToAction(nameof(EnableAuthenticator));
		}

		[HttpGet]
		public async Task<IActionResult> GenerateRecoveryCodesWarning()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!user.TwoFactorEnabled)
			{
				throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
			}

			return View(nameof(GenerateRecoveryCodes));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> GenerateRecoveryCodes()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!user.TwoFactorEnabled)
			{
				throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
			}

			var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
			_logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

			var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

			return View(nameof(ShowRecoveryCodes), model);
		}

		[Authorize(Roles = "Admin")]
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
		public IActionResult CreateUser()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
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
					string path = GetAvatarPath(model.AvatarFile);

					user.AvatarPath = "~/" + path;

					using (var fileStream = new FileStream(_appEnvironment.WebRootPath + "/" + path, FileMode.Create))
					{
						await model.AvatarFile.CopyToAsync(fileStream);
					}
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

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}
		
		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			var blogModeratros = await _context.BlogModerator.Where(m => m.ModeratorId == user.Id).ToListAsync();

			if (blogModeratros.Count() != 0)
			{
				foreach (var blogModerator in blogModeratros)
				{
					_context.BlogModerator.Remove(blogModerator);
				}
				await _context.SaveChangesAsync();
			}

			_context.UserLogins.RemoveRange(_context.UserLogins.Where(ul => ul.UserId == user.Id));

			_context.UserRoles.RemoveRange(_context.UserRoles.Where(ur => ur.UserId == user.Id));

			_context.Users.Remove(_context.Users.Where(usr => usr.Id == user.Id).Single());

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(GetAllUsers));
		}

		#region Helpers

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		private string FormatKey(string unformattedKey)
		{
			var result = new StringBuilder();
			int currentPosition = 0;
			while (currentPosition + 4 < unformattedKey.Length)
			{
				result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
				currentPosition += 4;
			}
			if (currentPosition < unformattedKey.Length)
			{
				result.Append(unformattedKey.Substring(currentPosition));
			}

			return result.ToString().ToLowerInvariant();
		}

		private string GenerateQrCodeUri(string email, string unformattedKey)
		{
			return string.Format(
				AuthenticatorUriFormat,
				_urlEncoder.Encode("ForIdentityCore2._0"),
				_urlEncoder.Encode(email),
				unformattedKey);
		}

		private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
		{
			var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
			if (string.IsNullOrEmpty(unformattedKey))
			{
				await _userManager.ResetAuthenticatorKeyAsync(user);
				unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
			}

			model.SharedKey = FormatKey(unformattedKey);
			model.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
		}

		private string GetAvatarPath(IFormFile avatar)
		{
			string fileName = Path.GetFileNameWithoutExtension(avatar.FileName);
			string extension = Path.GetExtension(avatar.FileName);
			fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

			return "Avatars/" + fileName;
		}

		#endregion
	}
}