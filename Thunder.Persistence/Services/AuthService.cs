using Microsoft.EntityFrameworkCore;
using Thunder.Application.Interfaces;
using Thunder.Application.Models;
using Thunder.Domain.Entities;
using Thunder.Persistence.Context;

namespace Thunder.Persistence.Services
{
	public class AuthService(AppDbContext appDbContext) : IAuthService
	{
		private readonly AppDbContext appDbContext = appDbContext;
		private DbSet<User> Users => appDbContext.Set<User>();

		public bool EmailCannotDuplicateWhenRegistered(string email)
		{
			return Users.Any(u => u.Email.ToLower().Trim() == email.ToLower().Trim());
		}

		public User GetUserByEmail(string email)
		{
			return Users.FirstOrDefault(u => u.Email == email)
				?? throw new KeyNotFoundException($"User with email '{email}' not found.");
		}

		public User GetUserByUsername(string username)
		{
			return Users.FirstOrDefault(u => u.Name == username) 
				?? throw new KeyNotFoundException($"User with username '{username}' not found.");
		}

		public bool IsAuthenticated(LoginModel model)
		{
			return Users.Any(u => u.Name == model.Username && u.PasswordHash == model.Password);
		}

		public async System.Threading.Tasks.Task RegisterAsync(User user)
		{
			Users.Add(user);
			await appDbContext.SaveChangesAsync();
		}

		public bool UsernameCannotDuplicateWhenRegistered(string username)
		{
			return Users.Any(u => u.Name.ToLower().Trim() == username.ToLower().Trim());
		}
	}
}
