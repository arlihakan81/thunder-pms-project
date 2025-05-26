using Thunder.Domain.Common;

namespace Thunder.Domain.Entities
{
	public class Team : TEntity<Guid> 
	{
		public required string Name { get; set; }
		public string? Description { get; set; }

		public Guid? ProjectId { get; set; }
		public Project? Project { get; set; }

		public List<User>? Members { get; set; }
	}
}