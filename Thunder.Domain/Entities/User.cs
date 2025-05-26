using System.ComponentModel.DataAnnotations;
using Thunder.Domain.Common;
using Thunder.Domain.Enums.Users;

namespace Thunder.Domain.Entities
{
	public class User : TEntity<Guid>
	{
		public Guid? ReferenceId { get; set; }
		public Reference? Reference { get; set; }
		public string? Avatar { get; set; }
		public Guid? TeamId { get; set; }
		public Team? Team { get; set; }
		public required string Name { get; set; }

		[DataType(DataType.EmailAddress)]
		public required string Email { get; set; }

		[DataType(DataType.Password)]
		public required string PasswordHash { get; set; }

		public Status Status { get; set; }

		public Role Role { get; set; }
		public List<Task>? Tasks { get; set; }
		public List<Team>? Teams { get; set; }
	}
}
