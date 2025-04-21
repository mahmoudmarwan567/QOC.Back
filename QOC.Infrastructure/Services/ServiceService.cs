using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs;
using QOC.Application.Interfaces;
using QOC.Domain.Entities;
using QOC.Infrastructure.Persistence;

namespace QOC.Infrastructure.Services
{
    public class ServiceService : IServiceService
    {
        private readonly ApplicationDbContext _context;

        public ServiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceDto>> GetAllAsync()
        {
            var services = await _context.Services.OrderBy(s => s.Order).ToListAsync();
            var serviceDto = services.Select(service => new ServiceDto
            {
                Id = service.Id,
                TitleAR = service.TitleAR,
                TitleEN = service.TitleEN,
                DescriptionAR = service.DescriptionAR,
                DescriptionEN = service.DescriptionEN,
                Order = service.Order,
                ImageUrl = service.ImageUrl
            }).ToList();
            return serviceDto;
        }

        public async Task<ServiceDto> GetByIdAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            return new ServiceDto
            {
                Id = service.Id,
                TitleAR = service.TitleAR,
                TitleEN = service.TitleEN,
                DescriptionAR = service.DescriptionAR,
                DescriptionEN = service.DescriptionEN,
                Order = service.Order,
                ImageUrl = service.ImageUrl
            };
        }

        public async Task<ServiceDto> CreateAsync(ServiceRequestDto dto)
        {
            var service = new Service
            {
                TitleAR = dto.TitleAR,
                TitleEN = dto.TitleEN,
                DescriptionAR = dto.DescriptionAR,
                DescriptionEN = dto.DescriptionEN,
                Order = dto.Order,
                ImageUrl = dto.ImageUrl
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return new ServiceDto
            {
                Id = service.Id,
                TitleAR = service.TitleAR,
                TitleEN = service.TitleEN,
                DescriptionAR = service.DescriptionAR,
                DescriptionEN = service.DescriptionEN,
                Order = service.Order,
                ImageUrl = service.ImageUrl
            };
        }

        public async Task<ServiceDto> UpdateAsync(int id, ServiceRequestDto dto)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return null;

            service.TitleAR = dto.TitleAR;
            service.TitleEN = dto.TitleEN;
            service.DescriptionAR = dto.DescriptionAR;
            service.DescriptionEN = dto.DescriptionEN;
            service.Order = dto.Order;
            service.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();
            return new ServiceDto
            {
                Id = service.Id,
                TitleAR = service.TitleAR,
                TitleEN = service.TitleEN,
                DescriptionAR = service.DescriptionAR,
                DescriptionEN = service.DescriptionEN,
                Order = service.Order,
                ImageUrl = service.ImageUrl
            };
        }

        public async Task DeleteAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return;

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
        }
    }
}

