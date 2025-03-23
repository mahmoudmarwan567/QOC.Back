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
    public class SliderService : ISliderService
    {
        private readonly ApplicationDbContext _context;

        public SliderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Slider>> GetAllSlidersAsync()
        {
            return await _context.Sliders.ToListAsync();
        }

        public async Task<Slider> GetSliderByIdAsync(int id)
        {
            return await _context.Sliders.FindAsync(id);
        }

        public async Task<Slider> CreateSliderAsync(SliderDto dto)
        {
            var slider = new Slider
            {
                Title = dto.Title,
                Subtitle = dto.Subtitle,
                ImageUrl = dto.ImageUrl,
                Description = dto.Description,
                ButtonText = dto.ButtonText,
                ButtonLink = dto.ButtonLink,
                IsActive = dto.IsActive
            };

            _context.Sliders.Add(slider);
            await _context.SaveChangesAsync();
            return slider;
        }

        public async Task<Slider> UpdateSliderAsync(int id, SliderDto dto)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return null;

            slider.Title = dto.Title;
            slider.Subtitle = dto.Subtitle;
            slider.ImageUrl = dto.ImageUrl;
            slider.Description = dto.Description;
            slider.ButtonText = dto.ButtonText;
            slider.ButtonLink = dto.ButtonLink;
            slider.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return slider;
        }

        public async Task DeleteSliderAsync(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return;

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
        }
    }
}

