using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thunder.Domain.Enums.Users;

namespace Thunder.Application.Dto.Users
{
    public class CreateUserDto
    {
		public Guid? ReferenceId { get; set; }
		public Guid? TeamId { get; set; }
		public string? Avatar { get; set; }
		public required string Name { get; set; }
		public required string Email { get; set; }
		public required string Password { get; set; }
		public Status Status { get; set; }
		public Role Role { get; set; }
	}
}
