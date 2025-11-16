using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.ProjectDtos;
using MyWebsite.Dtos.Response;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;
using MyWebsite.Validator.Project;

namespace MyWebsite.Service.Concrate
{
    public class ProjectManager : IProjectService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IValidator<CreateProjectDtos> _createProjectValidator;

        public ProjectManager(IRepositoryManager repositoryManager, IValidator<CreateProjectDtos> createProjectValidator)
        {
            _repositoryManager = repositoryManager;
            _createProjectValidator = createProjectValidator;
        }

        public async Task<BaseResponse<object>> CreateProjectAsync(CreateProjectDtos createProjectDtos)
        {
            var resultValidator = await _createProjectValidator.ValidateAsync(createProjectDtos);
            if (!resultValidator.IsValid)
            {
                return new BaseResponse<object>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrroCode = ErrorCodes.ValidationError,
                    Message = resultValidator.Errors.Select(error => error.ErrorMessage).FirstOrDefault()
                };
            }
            var project = new Project()
            {
                Title = createProjectDtos.Title,
                Description = createProjectDtos.Description,
                Technologies = createProjectDtos.Technologies,
                GithubUrl = createProjectDtos.GithubUrl,
                LiveDemoUrl = createProjectDtos.LiveDemoUrl,
                CategoryId = createProjectDtos.CategoryId

            };
            await _repositoryManager.ProjectRepository.AddAsync(project);
            await _repositoryManager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Data = null,
                Message = "proje başarılı bir şekilde eklendi",
                ErrroCode = null
            };
        }

        public async Task<BaseResponse<object>> DeleteProjectAsync(int id)
        {
            var project = await _repositoryManager.ProjectRepository.GetByIdAsync(id);
            if (project == null)
            {
                throw new NotFoundException($"{id} li proje bulunamadı");
            }
            await _repositoryManager.ProjectRepository.DeleteAsync(project);
            await _repositoryManager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Data = project,
                ErrroCode = null,
                Message = "proje başarılı bir şekilde silindi"
            };
        }

        public async Task<BaseResponse<IQueryable<ReadProjectDtos>>> GetAllProjectsAsync()
        {
            var projects = await _repositoryManager.ProjectRepository.FindAllProject().ToListAsync();

            var projectDtos = projects.Select(p => new ReadProjectDtos
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Technologies = p.Technologies,
                GithubUrl = p.GithubUrl,
                LiveDemoUrl = p.LiveDemoUrl,
                CategoryName = p.Category != null ? p.Category.Name : null
            }).AsQueryable();
            return new BaseResponse<IQueryable<ReadProjectDtos>>
            {
                IsSuccess = true,
                Data = projectDtos,
                Message = "projeler başarıyla çekildi",
                ErrroCode = null
            };

        }

        public async Task<BaseResponse<ReadProjectDtos?>> GetProjectByIdAsync(int id)
        {
            var project = await _repositoryManager.ProjectRepository.GetByIdAsyncProject(id);
            if (project is null)
            {
                throw new NotFoundException($"{id} li proje bulunamadı");
            }
            var projectDto = new ReadProjectDtos
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Technologies = project.Technologies,
                GithubUrl = project.GithubUrl,
                LiveDemoUrl = project.LiveDemoUrl,
                CategoryName = project.Category != null ? project.Category.Name : null
            };
            return new BaseResponse<ReadProjectDtos?>
            {
                IsSuccess = true,
                Data = projectDto,
                Message = "proje başarılı bir şekilde çekildi",
                ErrroCode = null
            };
        }

        public async Task<BaseResponse<object>> UpdateProjectAsync(int id, UpdateProjectDtos dto)
        {
            var project =await  _repositoryManager.ProjectRepository.GetByIdAsync(id);
            if (project == null)
            {
                throw new NotFoundException($"{id} li proje bulunamadı");
            }
            project.Title = dto.Title;
            project.LiveDemoUrl = dto.LiveDemoUrl;
            project.GithubUrl = dto.GithubUrl;
            project.Technologies = dto.Technologies;
            project.Description = dto.Description;
            
            await _repositoryManager.ProjectRepository.UpdateAsync(project);
            await _repositoryManager.SaveAsync();
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Data = project,
                ErrroCode = null,
                Message = "proje başarılı bir şekilde güncellendi"
            };
        }
    }
}
