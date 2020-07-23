using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;
using UrlShortener.Persistence.Contracts;

namespace UrlShortener.Persistence.Implementation.SQLServer
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>, IDisposable where TEntity : EntityBase
    {
        protected DataContext _dataContext;
        protected DbSet<TEntity> _entitySet;

        public RepositoryBase(DataContext dataContext)
        {
            _dataContext = dataContext;
            _entitySet = dataContext.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity)
        {
            try
            {
                await _entitySet.AddAsync(entity);
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Can't add {typeof( TEntity ).Name}", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        #region IDisposable 

        private bool disposed = false;

        protected virtual void Dispose( bool disposing )
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dataContext.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        #endregion
    }
}
