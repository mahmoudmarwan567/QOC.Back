using QOC.Application.DTOs;
using QOC.Application.DTOs.AboutUs;

namespace QOC.Application.Interfaces
{
    public interface IAboutUsService
    {
        Task<IEnumerable<AboutUsDto>> GetAllAsync();
        Task<AboutUsDto> GetByIdAsync(int id);
        Task<AboutUsDto> CreateAsync(AboutUsRequestDto dto);
        Task<AboutUsDto> UpdateAsync(int id, AboutUsRequestDto dto);
        Task DeleteAsync(int id);
    }
}
