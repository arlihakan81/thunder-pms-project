using Thunder.Application.Models;
using Thunder.Domain.Entities;

namespace Thunder.Application.Interfaces
{
	public interface IAuthService
    {

        bool IsAuthenticated(LoginModel model);
		User GetUserByUsername(string username);

		User GetUserByEmail(string email);
		bool EmailCannotDuplicateWhenRegistered(string email);
		bool UsernameCannotDuplicateWhenRegistered(string username);

		System.Threading.Tasks.Task RegisterAsync(User user);
	}
}
