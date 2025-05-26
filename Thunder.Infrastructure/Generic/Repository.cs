using Microsoft.EntityFrameworkCore;
using Thunder.Domain.Common;
using Thunder.Persistence.Context;

namespace Thunder.Infrastructure.Generic
{
	public class Repository<T>(AppDbContext appDbContext) : IRepository<T> where T : TEntity<Guid>
	{
		private readonly AppDbContext appDbContext = appDbContext;
		private DbSet<T> Table => appDbContext.Set<T>();

		public async Task AddAsync(T entity)
		{
			Table.Add(entity);
			await SaveAsync();
		}

		public async Task DeleteAsync(Guid id)
		{
			Table.Remove(GetById(id));
			await SaveAsync();
		}

		public List<T> GetAll()
		{
			return [.. Table];
		}

		public T GetById(Guid id)
		{
			return Table.FirstOrDefault(t => t.Id == id)!;
		}

		public async Task SaveAsync()
		{
			await appDbContext.SaveChangesAsync();
		}

		public async Task UpdateAsync(T entity)
		{
			Table.Update(entity);
			await SaveAsync();
		}
	}
}
