using Thunder.Domain.Common;

namespace Thunder.Domain.Entities
{
	public class Project : TEntity<Guid>
	{
		public required string Name { get; set; }
		public string? Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? DeadLine { get; set; }
		public decimal? Budget { get; set; }
		public decimal? Cost { get; set; }
		public decimal? SpentCost { get; set; }
		public int? SpentTime { get; set; }

		public List<Task>? Tasks { get; set; }
		public List<Team>? Teams { get; set; }

	}
}