using QOC.Application.DTOs.Section;

namespace QOC.Infrastructure.Contracts
{
    public interface ISectionService
    {
        Task<IEnumerable<SectionDto>> GetAllAsync();
        Task<SectionDto> GetByIdAsync(int id);
        Task<SectionDto> CreateAsync(SectionRequestDto dto);
        Task<SectionDto> UpdateAsync(int id, SectionRequestDto dto);
        Task DeleteAsync(int id);
    }
}
