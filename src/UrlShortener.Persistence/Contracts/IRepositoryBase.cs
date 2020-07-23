using System;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Persistence.Contracts
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : EntityBase
	{
		Task CreateAsync(TEntity entity);
		Task SaveChangesAsync();
	}
}