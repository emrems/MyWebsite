using MyWebsite.Dtos.MediaDtos;
using MyWebsite.Dtos.Response;

namespace MyWebsite.Service.İnterfaces
{
    public interface IMediService
    {
        Task<BaseResponse<object>> createMedia(CreateMediaDto dto);
    }
}
