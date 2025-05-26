using Thunder.Domain.Common;

namespace Thunder.Infrastructure.Generic
{
	public interface IRepository<T> where T : TEntity<Guid>
	{
		List<T> GetAll();
		T GetById(Guid id);

		Task AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(Guid id);
		Task SaveAsync();
	}
}
