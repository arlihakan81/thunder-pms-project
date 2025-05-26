using Microsoft.EntityFrameworkCore;
using Thunder.Domain.Entities;
using Thunder.Domain.Enums.Users;
using Thunder.Infrastructure.Generic;
using Thunder.Persistence.Context;

namespace Thunder.Infrastructure.Repositories.Users
{
	public class UserRepository(AppDbContext appDbContext) : Repository<User>(appDbContext), IUserRepository
	{
		private readonly AppDbContext appDbContext = appDbContext;
		private DbSet<User> Users => appDbContext.Set<User>();

		public List<User> GetMembersByProject(Guid projectId)
		{
			return [.. Users.Where(us => us.Role == Role.Member && us.Tasks!.Any(t => t.ProjectId == projectId))];
		}

		public User GetAssigneeByTask(Guid taskId)
		{
			return Users.Where(us => us.Tasks!.Any(t => t.Id == taskId))
				.FirstOrDefault()!;
		}

		public List<User> GetActiveUsers()
		{
			return [.. Users.Where(us => us.Status == Status.Active)];
		}

		public List<User> GetInactiveUsers()
		{
			return [.. Users.Where(us => us.Status == Status.Inactive)];
		}

		public List<User> GetLeadsByProject(Guid projectId)
		{
			return [.. Users.Where(us => us.Role == Role.Lead && us.Teams!.Any(t => t.ProjectId == projectId))];
		}

		public List<User> GetMembersByTeam(Guid teamId)
		{
			return [.. Users.Where(us => us.Role == Role.Member && us.TeamId == teamId)];
		}

		public bool NameCannotDuplicateWhenInserted(string name)
		{
			return Users.Any(u => u.Name.ToLower().Trim() == name.ToLower().Trim());
		}

		public bool NameCannotDuplicateWhenUpdated(Guid id, string name)
		{
			return Users.Any(u => u.Id != id && u.Name.ToLower().Trim() == name.ToLower().Trim());
		}

		public bool EmailCannotDuplicateWhenInserted(string email)
		{
			return Users.Any(u => u.Email!.ToLower().Trim() == email.ToLower().Trim());
		}

		public bool EmailCannotDuplicateWhenUpdated(Guid id, string email)
		{
			return Users.Any(u => u.Id != id && u.Email!.ToLower().Trim() == email.ToLower().Trim());
		}

		public List<User> GetUsersByReference(Guid referenceId)
		{
			return [.. Users.Where(us => us.ReferenceId == referenceId)];
		}
	}
}
