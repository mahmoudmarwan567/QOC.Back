using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _context.Services.OrderBy(s => s.Order).ToListAsync();
        }

        public async Task<Service> GetByIdAsync(int id)
        {
            return await _context.Services.FindAsync(id);
        }

        public async Task<Service> CreateAsync(ServiceDto dto)
        {
            var service = new Service
            {
                Title = dto.Title,
                Description = dto.Description,
                Order = dto.Order,
                ImageUrl = dto.ImageUrl
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<Service> UpdateAsync(int id, ServiceDto dto)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return null;

            service.Title = dto.Title;
            service.Description = dto.Description;
            service.Order = dto.Order;
            service.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();
            return service;
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

