namespace Thunder.Application.Interfaces
{
	public interface IUserService 
	{
		int GetActiveUsersCount();
		int GetInactiveUsersCount();
		int GetLeadsCountByProject(Guid projectId);
		int GetMembersCountByProject(Guid projectId);
		int GetMembersCountByTeam(Guid teamId);

	}
}
