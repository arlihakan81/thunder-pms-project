using Thunder.Domain.Entities;
using Thunder.Infrastructure.Generic;

namespace Thunder.Infrastructure.Repositories.References
{
	public interface IReferenceRepository : IRepository<Reference>	
	{
		Reference GetReferenceByUser(Guid userId);

	}
}
