using Thunder.Domain.Entities;
using Thunder.Infrastructure.Generic;

namespace Thunder.Infrastructure.Repositories.Users
{
	public interface IUserRepository : IRepository<User>
	{
		List<User> GetMembersByProject(Guid projectId);

		User GetAssigneeByTask(Guid taskId);

		List<User> GetActiveUsers();
		List<User> GetInactiveUsers();

		List<User> GetLeadsByProject(Guid projectId);
		List<User> GetMembersByTeam(Guid teamId);

		List<User> GetUsersByReference(Guid referenceId);

		bool NameCannotDuplicateWhenInserted(string name);
		bool NameCannotDuplicateWhenUpdated(Guid id, string name);

		bool EmailCannotDuplicateWhenInserted(string email);
		bool EmailCannotDuplicateWhenUpdated(Guid id, string email);
	}
}
