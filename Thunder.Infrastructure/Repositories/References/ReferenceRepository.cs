using Thunder.Domain.Entities;
using Thunder.Infrastructure.Generic;
using Thunder.Persistence.Context;

namespace Thunder.Infrastructure.Repositories.References
{
	public class ReferenceRepository(AppDbContext context) : Repository<Reference>(context), IReferenceRepository
	{
		private readonly AppDbContext context = context;

        public bool EmailAddressCannotDuplicateWhenInserted(string email)
        {
            return context.References.Any(r => r.EmailAddress.ToLower().Trim() == email.ToLower().Trim());
        }

        public bool EmailAddressCannotDuplicateWhenUpdated(Guid id, string email)
        {
            return context.References.Any(r => r.EmailAddress.ToLower().Trim() == email.ToLower().Trim() && r.Id != id);
        }

        public Reference GetReferenceByUser(Guid userId)
		{
			return context.References.FirstOrDefault(r => r.Users!.Any(u => u.Id == userId))!;
		}
	}
}
