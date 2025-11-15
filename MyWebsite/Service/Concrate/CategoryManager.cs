using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.ArticleDtos;
using MyWebsite.Dtos.CategoryDtos;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.ProjectDtos;
using MyWebsite.Dtos.Response;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class CategoryManager : ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IValidator<CreateCategoryDtos> _createCategoryValidator;
        public CategoryManager(IRepositoryManager repositoryManager, IValidator<CreateCategoryDtos> createCategoryValidator)
        {
            _repositoryManager = repositoryManager;
            _createCategoryValidator = createCategoryValidator;
        }
        public async Task<BaseResponse<object>> AddCategory(CreateCategoryDtos category)
        {
            var resultValidator = await _createCategoryValidator.ValidateAsync(category);
            if (!resultValidator.IsValid)
            {
                var errors = resultValidator.Errors.Select(e => e.ErrorMessage).FirstOrDefault();
                return new BaseResponse<object>
                {
                    IsSuccess = false,
                    Message = errors,
                    ErrroCode = ErrorCodes.ValidationError,
                    Data = null
                };
            }
            var categories = new Category
            {
              //  Id = category.Id,
                Name = category.Name,
                Slug = category.Slug
            };
            await _repositoryManager.CategoryRepository.AddAsync(categories);
            await _repositoryManager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Message = "Kategori başarıyla eklendi",
                Data = null,
                ErrroCode = null
            };

        }

        public async Task<BaseResponse<object>> DeleteCategory(int id)
        {
            var category = await _repositoryManager.CategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException("Category bulunamadı");
            }
            await _repositoryManager.CategoryRepository.DeleteAsync(category);
            await _repositoryManager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Message = "Kategori başarıyla silindi",
                Data = null,
                ErrroCode = null
            };
        }

        public async Task<BaseResponse<IEnumerable<ReadCategoryDtos>>> GetAllCategories()
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

            return new BaseResponse<IEnumerable<ReadCategoryDtos>>
            {
                IsSuccess = true,
                Message = "Kategoriler başarıyla getirildi",
                Data = categoryDtos,
                ErrroCode = null
            };
        }

        public async Task<BaseResponse<ReadCategoryDtos>> GetCategoryById(int id)
        {
            var categories = await _repositoryManager.CategoryRepository.FindCategoryByIdAsync(id);
            if (categories == null)
            {
                throw new NotFoundException("Category bulunamadı");
            }
            var categoryDto = new ReadCategoryDtos
            {
                Id = categories.Id,
                Name = categories.Name,
                Slug = categories.Slug,
                Articles = categories.Articles.Select(article => new ReadArticleDtos
                {
                    Id = article.Id,
                    Content = article.Content,
                    Title = article.Title,
                    Slug = article.Slug,
                    CreatedDate = article.CreatedDate
                }).ToList(),

                Projects = categories.Projects.Select(prj => new ReadProjectDtos
                {
                    Id = prj.Id,
                    Technologies = prj.Technologies,
                    Title = prj.Title,
                    Description = prj.Description,
                    GithubUrl = prj.GithubUrl,
                    LiveDemoUrl = prj.LiveDemoUrl,
                    CategoryName = prj.Category.Name

                }).ToList()
            };
            return new BaseResponse<ReadCategoryDtos>
            {
                IsSuccess = true,
                Data = categoryDto,
                Message = "categori başarılı bir şekilde çekildi",
                ErrroCode = null
            };


        }

        public async Task<BaseResponse<object>> UpdateCategory(UpdateCategoryDtos dto)
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
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Message = "Kategori başarıyla güncellendi",
                Data = null,
                ErrroCode = null
            };
        }
    }
}
