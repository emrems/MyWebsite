using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.ArticleDtos;
using MyWebsite.Dtos.CategoryDtos;
using MyWebsite.Dtos.ProjectDtos;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class CategoryManager : ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        public CategoryManager(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }
        public async Task AddCategory(CreateCategoryDtos category)
        {
            var categories = new Category
            {
              //  Id = category.Id,
                Name = category.Name,
                Slug = category.Slug
            };
            await _repositoryManager.CategoryRepository.AddAsync(categories);
            await _repositoryManager.SaveAsync();

        }

        public async Task DeleteCategory(int id)
        {
            var category = await _repositoryManager.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException("Category bulunamadı");
            }
            await _repositoryManager.CategoryRepository.DeleteAsync(category);
            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<ReadCategoryDtos>> GetAllCategories()
        {
            var categories = await _repositoryManager.CategoryRepository
                .FindAllWithRelations()
                .ToListAsync();

            var categoryDtos = categories.Select(c => new ReadCategoryDtos
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,

                // Article mapping
                Articles = c.Articles.Select(a => new ReadArticleDtos
                {
                    Title = a.Title,
                    Content = a.Content,
                    Slug = a.Slug,
                    CreatedDate = a.CreatedDate
                }).ToList(),

                // Project mapping
                Projects = c.Projects.Select(p => new ReadProjectDtos
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    GithubUrl = p.GithubUrl,
                    LiveDemoUrl = p.LiveDemoUrl,
                    Technologies = p.Technologies,
                    CategoryName = p.Category != null ? p.Category.Name : null
                }).ToList()
            });

            return categoryDtos;
        }



        public async Task UpdateCategory(UpdateCategoryDtos dto)
        {
            var category = await _repositoryManager.CategoryRepository.GetByIdAsync(dto.Id);
            if (category == null)
            {
                throw new NotFoundException("Category bulunamadı");
            }
            category.Name = dto.Name;
            category.Slug = dto.Slug;

            await _repositoryManager.CategoryRepository.UpdateAsync(category);
            await _repositoryManager.SaveAsync();
        }
    }
}
