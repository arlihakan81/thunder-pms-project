using Thunder.Domain.Enums.Tasks;
using Thunder.Infrastructure.Generic;

namespace Thunder.Infrastructure.Repositories.Tasks
{
	public interface ITaskRepository : IRepository<Domain.Entities.Task>
	{
		List<Domain.Entities.Task> GetTasksByAssignee(Guid assignee);
		List<Domain.Entities.Task> GetTasksByProject(Guid projectId);
		List<Domain.Entities.Task> GetTasksByStatus(Status status);
		List<Domain.Entities.Task> GetCurrentTasks();
		List<Domain.Entities.Task> GetTasksByTag(Guid tagId);
		Domain.Entities.Task GetTaskBySubtask(Guid subtaskId);


	}
}
