using QOC.Application.DTOs.ProjectCategory;

namespace QOC.Infrastructure.Contracts
{
    public interface IProjectCategoryService
    {
        Task<List<ResponseProjectCategoryDto>> GetAllAsync();
        Task<ResponseProjectCategoryDto?> GetByIdAsync(int id);
        Task<ResponseProjectCategoryDto> CreateAsync(RequestProjectCategoryDto dto);
        Task<ResponseProjectCategoryDto?> UpdateAsync(int id, RequestProjectCategoryDto dto);
        Task DeleteAsync(int id);
        Task<List<ResponseBasicProjectCategoryDto>> GetAllWithoutProjectsAsync();
    }
}
