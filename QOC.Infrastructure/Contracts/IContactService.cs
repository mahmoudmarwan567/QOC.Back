using QOC.Application.DTOs;
using QOC.Domain.Entities;

namespace QOC.Infrastructure.Contracts
{
    public interface IContactService
    {
        Task<ContactMessageDto> AddAsync(ContactMessage message);
        Task<IEnumerable<ContactMessageDto>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<ContactMessageDto?> GetByIdAsync(int id);
        Task MarkAsReadAsync(int id);
        Task DeleteAsync(int id);
        Task<int> GetUnreadCountAsync();
    }
}
