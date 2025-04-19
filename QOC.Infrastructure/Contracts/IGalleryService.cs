using QOC.Application.DTOs.Gallery;

namespace QOC.Infrastructure.Contracts
{
    public interface IGalleryService
    {
        Task<IEnumerable<GalleryDto>> GetAllAsync();
        Task<List<GalleryDto>> BulkCreateAsync(IEnumerable<GalleryRequestDto> gallery);
        Task DeleteAsync(int id);
    }
}
