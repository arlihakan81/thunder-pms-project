using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Thunder.Application.Interfaces;
using Thunder.Application.Models;
using Thunder.Domain.Entities;
using Thunder.Infrastructure.Identity;
using Thunder.Infrastructure.Repositories.Users;

namespace Thunder.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(IAuthService authService) : ControllerBase
	{
		private readonly IAuthService authService = authService;

		[HttpPost("Login")]
		public async Task<ActionResult> LoginAsync([FromBody] LoginModel model)
		{
			if(!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Please enter your username and password");
				return BadRequest(ModelState);
            }

			if(string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("", "Username and password cannot be empty");
                return BadRequest(ModelState);
            }

            var user = authService.GetUserByUsername(model.Username);
			if(user == null)
			{
				ModelState.AddModelError(model.Username, "Username not found");
				return NotFound($"{model.Username} not found");
			}
            
			if(new PasswordHasher().Verify(model.Password, user.PasswordHash)
                == PasswordVerificationResult.Failed)
			{
                ModelState.AddModelError("", "Invalid username or password");
                return BadRequest("Invalid username or password");
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, CreateClaims(user));
            return Ok("You logged in");
		}

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please enter your username and password");
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("", "Username and password cannot be empty");
                return BadRequest(ModelState);
            }
            if (authService.UsernameCannotDuplicateWhenRegistered(model.Username))
            {
                ModelState.AddModelError(model.Username, "Username already exists");
                return BadRequest("Username already exists");
            }
            if (authService.EmailCannotDuplicateWhenRegistered(model.Email))
            {
                ModelState.AddModelError(model.Email, "Email already exists");
                return BadRequest("Email already exists");
            }
            var user = new User
            {
                Name = model.Username,
                PasswordHash = new PasswordHasher().Hash(model.Password),
                Email = model.Email
            };

            await authService.RegisterAsync(user);
            return Ok("Your account saved");
        }


        private static ClaimsPrincipal CreateClaims(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}
