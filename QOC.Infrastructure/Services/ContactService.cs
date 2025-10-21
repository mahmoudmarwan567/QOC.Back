using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs;
using QOC.Domain.Entities;
using QOC.Infrastructure.Contracts;
using QOC.Infrastructure.Persistence;

namespace QOC.Infrastructure.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;

        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ContactMessageDto> AddAsync(ContactMessage message)
        {
            message.SubmittedDate = DateTime.UtcNow;
            message.IsRead = false;

            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            return MapToDto(message);
        }

        public async Task<IEnumerable<ContactMessageDto>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            var messages = await _context.ContactMessages
                .OrderByDescending(m => m.SubmittedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return messages.Select(MapToDto);
        }

        public async Task<ContactMessageDto?> GetByIdAsync(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            return message == null ? null : MapToDto(message);
        }

        public async Task MarkAsReadAsync(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                message.IsRead = true;
                message.ReadDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                _context.ContactMessages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetUnreadCountAsync()
        {
            return await _context.ContactMessages.CountAsync(m => !m.IsRead);
        }

        private ContactMessageDto MapToDto(ContactMessage message)
        {
            return new ContactMessageDto
            {
                Id = message.Id,
                Name = message.Name,
                Email = message.Email,
                Message = message.Message,
                AttachmentPath = message.AttachmentPath,
                AttachmentOriginalName = message.AttachmentOriginalName,
                SubmittedDate = message.SubmittedDate,
                IsRead = message.IsRead,
                ReadDate = message.ReadDate
            };
        }
    }
}
