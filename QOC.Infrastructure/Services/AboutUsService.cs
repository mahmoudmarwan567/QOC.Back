using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs;
using QOC.Application.DTOs.AboutUs;
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

        public async Task<IEnumerable<AboutUsDto>> GetAllAsync()
        {
            var aboutUs = await _context.AboutUs.ToListAsync();
            return aboutUs.Select(a => new AboutUsDto
            {
                Id = a.Id,
                DescriptionAR = a.DescriptionAR,
                DescriptionEN = a.DescriptionEN,
                FullDescriptionAR = a.FullDescriptionAR,
                FullDescriptionEN = a.FullDescriptionEN,
                ImageUrl = a.ImageUrl
            }).ToList();
        }

        public async Task<AboutUsDto> GetByIdAsync(int id)
        {
            var aboutUs = await _context.AboutUs.FindAsync(id);
            if (aboutUs == null) return null;
            return new AboutUsDto
            {
                Id = aboutUs.Id,
                DescriptionAR = aboutUs.DescriptionAR,
                DescriptionEN = aboutUs.DescriptionEN,
                FullDescriptionAR = aboutUs.FullDescriptionAR,
                FullDescriptionEN = aboutUs.FullDescriptionEN,
                ImageUrl = aboutUs.ImageUrl
            };
        }

        public async Task<AboutUsDto> CreateAsync(AboutUsRequestDto dto)
        {
            var aboutUs = new AboutUs
            {
                DescriptionAR = dto.DescriptionAR,
                DescriptionEN = dto.DescriptionEN,
                FullDescriptionAR = dto.FullDescriptionAR,
                FullDescriptionEN = dto.FullDescriptionEN,
                ImageUrl = dto.ImageUrl
            };

            _context.AboutUs.Add(aboutUs);
            await _context.SaveChangesAsync();
            return new AboutUsDto
            {
                Id = aboutUs.Id,
                DescriptionAR = aboutUs.DescriptionAR,
                DescriptionEN = aboutUs.DescriptionEN,
                FullDescriptionAR = aboutUs.FullDescriptionAR,
                FullDescriptionEN = aboutUs.FullDescriptionEN,
                ImageUrl = aboutUs.ImageUrl
            };
        }

        public async Task<AboutUsDto> UpdateAsync(int id, AboutUsRequestDto dto)
        {
            var aboutUs = await _context.AboutUs.FindAsync(id);
            if (aboutUs == null) return null;

            aboutUs.DescriptionAR = dto.DescriptionAR;
            aboutUs.DescriptionEN = dto.DescriptionEN;
            aboutUs.FullDescriptionAR = dto.FullDescriptionAR;
            aboutUs.FullDescriptionEN = dto.FullDescriptionEN;
            aboutUs.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();
            return new AboutUsDto
            {
                Id = aboutUs.Id,
                DescriptionAR = aboutUs.DescriptionAR,
                DescriptionEN = aboutUs.DescriptionEN,
                FullDescriptionAR = aboutUs.FullDescriptionAR,
                FullDescriptionEN = aboutUs.FullDescriptionEN,
                ImageUrl = aboutUs.ImageUrl
            };
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
