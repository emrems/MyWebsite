using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.ProjectDtos;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class ProjectManager : IProjectService
    {
        private readonly IRepositoryManager _repositoryManager;

        public ProjectManager(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<CreateProjectDtos> CreateProjectAsync(CreateProjectDtos createProjectDtos)
        {
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
            return new CreateProjectDtos
            {
                Title = createProjectDtos.Title,
                Description = createProjectDtos.Description,
                Technologies = createProjectDtos.Technologies,
                GithubUrl = createProjectDtos.GithubUrl,
                LiveDemoUrl = createProjectDtos.LiveDemoUrl,
                CategoryId = createProjectDtos.CategoryId

            };
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _repositoryManager.ProjectRepository.GetByIdAsync(id);
            if (project == null)
            {
                throw new NotFoundException($"{id} li proje bulunamadı");
            }
            await _repositoryManager.ProjectRepository.DeleteAsync(project);
            await _repositoryManager.SaveAsync();
        }

        public async Task<IQueryable<ReadProjectDtos>> GetAllProjectsAsync()
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
            return projectDtos;

        }

        public async Task<ReadProjectDtos?> GetProjectByIdAsync(int id)
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
            return projectDto;
        }

        public async Task UpdateProjectAsync(int id)
        {
            var project =await  _repositoryManager.ProjectRepository.GetByIdAsync(id);
            if (project == null)
            {
                throw new NotFoundException($"{id} li proje bulunamadı");
            }
            await _repositoryManager.ProjectRepository.UpdateAsync(project);
            await _repositoryManager.SaveAsync();
        }
    }
}
