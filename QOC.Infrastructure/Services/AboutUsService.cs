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
    public class AboutUsService : IAboutUsService
    {
        private readonly ApplicationDbContext _context;

        public AboutUsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AboutUs>> GetAllAsync()
        {
            return await _context.AboutUs.ToListAsync();
        }

        public async Task<AboutUs> GetByIdAsync(int id)
        {
            return await _context.AboutUs.FindAsync(id);
        }

        public async Task<AboutUs> CreateAsync(AboutUsDto dto)
        {
            var aboutUs = new AboutUs
            {
                Description = dto.Description,
                FullDescription = dto.FullDescription,
                ImageUrl = dto.ImageUrl
            };

            _context.AboutUs.Add(aboutUs);
            await _context.SaveChangesAsync();
            return aboutUs;
        }

        public async Task<AboutUs> UpdateAsync(int id, AboutUsDto dto)
        {
            var aboutUs = await _context.AboutUs.FindAsync(id);
            if (aboutUs == null) return null;

            aboutUs.Description = dto.Description;
            aboutUs.FullDescription = dto.FullDescription;
            aboutUs.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();
            return aboutUs;
        }

        public async Task DeleteAsync(int id)
        {
            var aboutUs = await _context.AboutUs.FindAsync(id);
            if (aboutUs == null) return;

            _context.AboutUs.Remove(aboutUs);
            await _context.SaveChangesAsync();
        }
    }
}
