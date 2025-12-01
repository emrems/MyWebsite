using MyWebsite.Dtos.MediaDtos;
using MyWebsite.Dtos.Response;
using MyWebsite.Entities;
using MyWebsite.Repository.Interfaces;
using MyWebsite.Service.İnterfaces;

namespace MyWebsite.Service.Concrate
{
    public class MediaManager : IMediService
    {
        private readonly IRepositoryManager _manager;
        private readonly IWebHostEnvironment _env;

        public MediaManager(IRepositoryManager manager, IWebHostEnvironment env)
        {
            _manager = manager;
            _env = env;
        }

        public async Task<BaseResponse<object>> createMedia(CreateMediaDto dto)
        {
            var articleEmpty = dto.ArticleId == null || dto.ArticleId == 0;
            var projectEmpty = dto.ProjectId == null || dto.ProjectId == 0;
            if ((dto.ArticleId == null && dto.ProjectId == null)
                || (dto.ArticleId != null && dto.ProjectId != null))
            {
               
                return new BaseResponse<object>
                {
                    IsSuccess = false,
                    Message = "Media ya bir Article'a ya da bir Project'e bağlı olmalıdır.",
                    Data = null
                };
            }

            var uploadedMedias = new List<Media>();

            foreach (var file in dto.Files)
            {
                // 1. Dosya adı oluştur
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // 2. Klasör oluştur
                var folder = Path.Combine(_env.WebRootPath, "uploads/media");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // 3. Dosyayı kaydet
                var filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 4. Media entity oluştur
                var media = new Media
                {
                    Url = $"/uploads/media/{fileName}",
                    Type = file.ContentType.StartsWith("image")
                        ? Enum.MediaType.Image
                        : Enum.MediaType.Video,
                    ArticleId = dto.ArticleId,    
                    ProjectId = dto.ProjectId     
                };

                uploadedMedias.Add(media);
               await  _manager.MediaRepository.AddAsync (media);
            }

            await _manager.SaveAsync();

            return new BaseResponse<object>
            {
                IsSuccess = true,
                Message = "Media başarıyla yüklendi",
                Data = uploadedMedias.Select(m => new { m.Id, m.Url, m.Type })
            };
        }


    }
}
