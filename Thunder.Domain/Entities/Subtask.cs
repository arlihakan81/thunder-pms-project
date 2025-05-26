using Thunder.Domain.Common;

namespace Thunder.Domain.Entities
{
	public class Subtask : TEntity<Guid>
	{
		public Guid TaskId { get; set; }
		public required string Name { get; set; }
		public bool IsCompleted { get; set; }
		public Task? Task { get; set; }
	}
}
