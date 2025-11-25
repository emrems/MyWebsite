using MyWebsite.Entities;
using System.Linq.Expressions;

namespace MyWebsite.Repository.Interfaces
{
    public interface IRepositoryBase<TEntity>
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);
        IQueryable<TEntity> FindAll();
       

    }
}
