using MyWebsite.Entities;

namespace MyWebsite.Repository.Interfaces
{
    public interface ICategoryRepository : IRepositoryBase<Category>

    {
        IQueryable<Category> FindAllWithRelations();


    }
}
