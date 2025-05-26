using Microsoft.EntityFrameworkCore;
using Thunder.Application.Interfaces;
using Thunder.Domain.Entities;
using Thunder.Domain.Enums.Users;
using Thunder.Persistence.Context;

namespace Thunder.Persistence.Services
{
	public class UserService(AppDbContext appDbContext) : IUserService
	{
		private readonly AppDbContext appDbContext = appDbContext;
		private DbSet<User> Users => appDbContext.Set<User>();

		public int GetActiveUsersCount()
		{
			return Users.Count(user => user.Status == Status.Active);
		}

		public int GetInactiveUsersCount()
		{
			return Users.Count(user => user.Status == Status.Inactive);
		}

		public int GetLeadsCountByProject(Guid projectId)
		{
			return Users.Include(user => user.Tasks)
				.Where(user => user.Tasks.Any(t => t.ProjectId == projectId && user.Role == Role.Lead))
				.Count();
		}

		public int GetMembersCountByProject(Guid projectId)
		{
			return Users.Include(user => user.Tasks)
				.Where(user => user.Tasks.Any(t => t.ProjectId == projectId && user.Role == Role.Member))
				.Count();
		}

		public int GetMembersCountByTeam(Guid teamId)
		{
			return Users.Include(user => user.Teams)
				.Where(user => user.Teams.Any(t => t.Id == teamId && user.Role == Role.Member))
				.Count();
		}
	}
}
