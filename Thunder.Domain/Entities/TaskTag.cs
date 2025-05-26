using Thunder.Domain.Common;

namespace Thunder.Domain.Entities
{
	public class TaskTag : TEntity<Guid>
	{
		public Guid TaskId { get; set; }
		public Task? Task { get; set; }
		public Guid TagId { get; set; }
		public Tag? Tag { get; set; }
	}
}
