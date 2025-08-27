using MyWebsite.Repository.config;
using MyWebsite.Repository.Interfaces;

namespace MyWebsite.Repository.Concrate
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class
    {
        protected readonly MyWebSiteData _DbContext;

        public RepositoryBase(MyWebSiteData dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _DbContext.Set<TEntity>().AddAsync(entity);
           
        }

        public Task DeleteAsync(TEntity entity)
        {
           
            _DbContext.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public IQueryable<TEntity> FindAll()
        {
            return _DbContext.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _DbContext.Set<TEntity>().FindAsync(id);
        }

        public Task UpdateAsync(TEntity entity)
        {
            // Update metodu async değil, bu yüzden Task döndürmek için Task.CompletedTask kullanılır.
            _DbContext.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }
    }
}