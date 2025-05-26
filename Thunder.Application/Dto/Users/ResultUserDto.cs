using Thunder.Domain.Enums.Users;

namespace Thunder.Application.Dto.Users
{
	public class ResultUserDto
	{
		public Guid Id { get; set; }
		public string? Avatar { get; set; }
		public Guid? ReferenceId { get; set; }
		public Guid? TeamId { get; set; }
		public required string Name { get; set; }
		public required string Email { get; set; }
		public required string PasswordHash { get; set; }
		public Status Status { get; set; }
		public Role Role { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
