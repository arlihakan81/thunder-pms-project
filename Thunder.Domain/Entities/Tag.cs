using Thunder.Domain.Common;

namespace Thunder.Domain.Entities
{
	public class Tag : TEntity<Guid>
	{
		public required string Name { get; set; }

		public List<Domain.Entities.Task>? Tasks { get; set; }
		public List<TaskTag>? TaskTags { get; set; }
	}
}
