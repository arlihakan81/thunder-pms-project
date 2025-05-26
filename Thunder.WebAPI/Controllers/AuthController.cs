using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Thunder.Application.Interfaces;
using Thunder.Application.Models;
using Thunder.Infrastructure.Identity;
using Thunder.Infrastructure.Repositories.Users;

namespace Thunder.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(IAuthService authService, PasswordHasher hasher) : ControllerBase
	{
		private readonly IAuthService authService = authService;
		private readonly PasswordHasher hasher = hasher;

		[HttpPost("Login")]
		public async Task<ActionResult> LoginAsync([FromBody] LoginModel model)
		{
			if (model == null)
			{
				return BadRequest("Login model cannot be null.");
			}

			if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
			{
				return BadRequest("Username and password cannot be empty.");
			}

			model.Password = hasher.Hash(model.Password);
			if (!authService.IsAuthenticated(model))
			{
				return Unauthorized("Invalid username or password.");
			}

			var user = authService.GetUserByUsername(model.Username);
			if (user == null)
			{
				return NotFound($"User with username '{model.Username}' not found.");
			}

			List<Claim> claims = new()
			{
				new Claim(ClaimTypes.Name, user.Name),
				new Claim(ClaimTypes.Role, user.Role.ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
				new Claim("Avatar", user.Avatar ?? string.Empty),
				new Claim("TeamId", user.TeamId?.ToString() ?? string.Empty),
				new Claim("Status", user.Status.ToString())
			};

			ClaimsIdentity identity = new(claims,CookieAuthenticationDefaults.AuthenticationScheme);
			ClaimsPrincipal principal = new(identity);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
			return Ok("You logged in successfully");
		}

		[HttpPost("Logout")]
		public async Task<ActionResult> LogoutAsync()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Ok("You logged out successfully");
		}

		[HttpPost("Register")]
		public async Task<ActionResult> RegisterAsync(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				if (authService.UsernameCannotDuplicateWhenRegistered(model.Username))
				{
					return BadRequest($"Username '{model.Username}' is already taken.");
				}

				if (authService.EmailCannotDuplicateWhenRegistered(model.Email))
				{
					return BadRequest($"Email '{model.Email}' is already registered.");
				}

				var user = new Domain.Entities.User
				{
					Name = model.Username,
					Email = model.Email,
					PasswordHash = hasher.Hash(model.Password),
					Role = Domain.Enums.Users.Role.Unassigned,
					Status = Domain.Enums.Users.Status.Inactive,
					CreatedAt = DateTime.Now
				};

				await authService.RegisterAsync(user);
				return Ok("You registered successfully.");
			}
			else
			{
				return BadRequest(ModelState);
			}
		}




	}
}
