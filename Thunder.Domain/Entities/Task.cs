using Thunder.Domain.Common;
using Thunder.Domain.Enums.Tasks;

namespace Thunder.Domain.Entities
{
	public class Task : TEntity<Guid>
	{
		public Guid? ProjectId { get; set; }
		public required string Name { get; set; }
		public string? Description { get; set; }
		public Status Status { get; set; }
		public Priority Priority { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public Guid? AssigneeId { get; set; }

		public Project? Project { get; set; }
		public User? Assignee { get; set; }
		public List<Tag>? Tags { get; set; }
		public List<TaskTag>? TaskTags { get; set; } 
		public List<Subtask>? Subtasks { get; set; }
	}
}
