using System.ComponentModel.DataAnnotations;
using Thunder.Domain.Common;

namespace Thunder.Domain.Entities
{
	public class Reference : TEntity<Guid>
	{
		public required string Name { get; set; }

		[DataType(DataType.EmailAddress)]
		public required string EmailAddress { get; set; }

		public List<User>? Users { get; set; }
	}
}
