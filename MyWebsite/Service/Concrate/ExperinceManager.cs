using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Dtos.Error;
using MyWebsite.Dtos.Experince;
using MyWebsite.Dtos.Response;
using MyWebsite.Entities;
using MyWebsite.Exceptions;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class ExperinceManager : IExperinceServices
    {
        private readonly IRepositoryManager _manager;
        private readonly IValidator<CreateExperinceDtos> _createExperienceValidate;

        public ExperinceManager(IRepositoryManager manager, IValidator<CreateExperinceDtos> createExperienceValidate)
        {
            _manager = manager;
            _createExperienceValidate = createExperienceValidate;
        }

        public async Task<BaseResponse<object>> createExperienceAsync(CreateExperinceDtos dto)
        {
            var result = await _createExperienceValidate.ValidateAsync(dto);
            if (!result.IsValid)
            {
                return new BaseResponse<object>
                {
                    IsSuccess = false,
                    Data = null,
                    ErrroCode = ErrorCodes.ValidationError,
                    Message = result.Errors.Select(er => er.ErrorMessage).FirstOrDefault()
                };
            }
            var experince = new Experience
            {
                Company = dto.Company,
                Role = dto.Role,
                EndDate = dto.EndDate,
                StartDate = dto.StartDate,
                Description = dto.Description
            };
            await _manager.ExperinceRepository.AddAsync(experince);
           
            return new BaseResponse<object>
            {
                IsSuccess = true,
                Data = null,
                ErrroCode = null,
                Message = "deneyim başarılı bir şekilde oluşturuldu"

            };
        }

        public async Task<BaseResponse<IEnumerable<ReadExperinceDto>>> GetAllExperinceAsync()
        {
            var allExperinceDb = await _manager.ExperinceRepository.FindAll().ToListAsync();
            var allExperinceDto = allExperinceDb.Select(expr => new ReadExperinceDto
            {
                Company = expr.Company,
                Role = expr.Role,
                EndDate = expr.EndDate,
                StartDate = expr.StartDate,
                Description = expr.Description
            });

            return new BaseResponse<IEnumerable<ReadExperinceDto>>
            {
                IsSuccess = true,
                Data = allExperinceDto,
                ErrroCode = null,
                Message = "deneyimler başarılı bir şekilde çekildi"
            };
        }

        public async Task<BaseResponse<ReadExperinceDto>> GetExperinceById(int id)
        {
            var experinceDb = await _manager.ExperinceRepository.GetByIdAsync(id);
            if(experinceDb == null)
            {
                throw new NotFoundException("deneyim bulunamadı");
            }
            var experienceDto = new ReadExperinceDto
            {
                Company = experinceDb.Company,
                Role = experinceDb.Role,
                EndDate = experinceDb.EndDate,
                StartDate = experinceDb.StartDate,
                Description = experinceDb.Description

            };
            return new BaseResponse<ReadExperinceDto>
            {
                IsSuccess = true,
                Data = experienceDto,
                ErrroCode = null,
                Message = "deneyim başarılı bir şekilde çekildi"

            };
        }
    }
}
