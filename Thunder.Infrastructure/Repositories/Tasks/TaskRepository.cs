using Microsoft.EntityFrameworkCore;
using Thunder.Domain.Enums.Tasks;
using Thunder.Infrastructure.Generic;
using Thunder.Persistence.Context;

namespace Thunder.Infrastructure.Repositories.Tasks
{
	public class TaskRepository(AppDbContext context) : Repository<Domain.Entities.Task>(context), ITaskRepository
	{
		private readonly AppDbContext context = context;
		private DbSet<Domain.Entities.Task> Tasks => context.Set<Domain.Entities.Task>();

		public List<Domain.Entities.Task> GetCurrentTasks()
		{
			return [.. Tasks.OrderByDescending(t => t.CreatedAt)];
		}

		public Domain.Entities.Task GetTaskBySubtask(Guid subtaskId)
		{
			return Tasks.Include(t => t.Subtasks)
				.FirstOrDefault(t => t.Subtasks!.Any(st => st.Id == subtaskId))
				?? throw new KeyNotFoundException($"Task with subtask ID {subtaskId} not found.");
		}

		public List<Domain.Entities.Task> GetTasksByAssignee(Guid assignee)
		{
			return [.. Tasks.Where(t => t.AssigneeId == assignee)];
		}

		public List<Domain.Entities.Task> GetTasksByProject(Guid projectId)
		{
			return [.. Tasks.Where(t => t.ProjectId == projectId)];
		}

		public List<Domain.Entities.Task> GetTasksByStatus(Status status)
		{
			return [.. Tasks.Where(t => t.Status == status)];
		}

		public List<Domain.Entities.Task> GetTasksByTag(Guid tagId)
		{
			return [.. Tasks
				.Include(t => t.TaskTags)
				.Where(t => t.TaskTags!.Any(tt => tt.TagId == tagId))];
		}
	}
}
