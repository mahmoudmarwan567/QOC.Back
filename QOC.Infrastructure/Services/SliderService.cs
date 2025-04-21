using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs;
using QOC.Application.DTOs.Slider;
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

        public async Task<IEnumerable<SliderDto>> GetAllSlidersAsync()
        {
            var sliders = await _context.Sliders.ToListAsync();
            return sliders.Select(s => new SliderDto
            {
                Id = s.Id,
                TitleAR = s.TitleAR,
                TitleEN = s.TitleEN,
                SubtitleAR = s.SubtitleAR,
                SubtitleEN = s.SubtitleEN,
                ImageUrl = s.ImageUrl,
                DescriptionEN = s.DescriptionEN,
                DescriptionAR = s.DescriptionAR,
                ButtonTextAR = s.ButtonTextAR,
                ButtonTextEN = s.ButtonTextEN,
                ButtonLink = s.ButtonLink,
                IsActive = s.IsActive
            }).ToList();
        }

        public async Task<SliderDto> GetSliderByIdAsync(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return null;
            return new SliderDto
            {
                Id = slider.Id,
                TitleAR = slider.TitleAR,
                TitleEN = slider.TitleEN,
                SubtitleAR = slider.SubtitleAR,
                SubtitleEN = slider.SubtitleEN,
                ImageUrl = slider.ImageUrl,
                DescriptionEN = slider.DescriptionEN,
                DescriptionAR = slider.DescriptionAR,
                ButtonTextAR = slider.ButtonTextAR,
                ButtonTextEN = slider.ButtonTextEN,
                ButtonLink = slider.ButtonLink,
                IsActive = slider.IsActive
            };
        }

        public async Task<SliderDto> CreateSliderAsync(SliderRequestDto dto)
        {
            var slider = new Slider
            {
                TitleEN = dto.TitleEN,
                TitleAR = dto.TitleAR,
                SubtitleEN = dto.SubtitleEN,
                SubtitleAR = dto.SubtitleAR,
                ImageUrl = dto.ImageUrl,
                DescriptionEN = dto.DescriptionEN,
                DescriptionAR = dto.DescriptionAR,
                ButtonTextEN = dto.ButtonTextEN,
                ButtonTextAR = dto.ButtonTextAR,
                ButtonLink = dto.ButtonLink,
                IsActive = dto.IsActive
            };

            _context.Sliders.Add(slider);
            await _context.SaveChangesAsync();
            return new SliderDto
            {
                Id = slider.Id,
                TitleAR = slider.TitleAR,
                TitleEN = slider.TitleEN,
                SubtitleAR = slider.SubtitleAR,
                SubtitleEN = slider.SubtitleEN,
                ImageUrl = slider.ImageUrl,
                DescriptionEN = slider.DescriptionEN,
                DescriptionAR = slider.DescriptionAR,
                ButtonTextAR = slider.ButtonTextAR,
                ButtonTextEN = slider.ButtonTextEN,
                ButtonLink = slider.ButtonLink,
                IsActive = slider.IsActive
            };
        }

        public async Task<SliderDto> UpdateSliderAsync(int id, SliderRequestDto dto)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return null;

            slider.ButtonLink = dto.ButtonLink;
            slider.IsActive = dto.IsActive;
            slider.TitleEN = dto.TitleEN;
            slider.TitleAR = dto.TitleAR;
            slider.SubtitleEN = dto.SubtitleEN;
            slider.SubtitleAR = dto.SubtitleAR;
            slider.ImageUrl = dto.ImageUrl;
            slider.DescriptionEN = dto.DescriptionEN;
            slider.DescriptionAR = dto.DescriptionAR;
            slider.ButtonTextEN = dto.ButtonTextEN;
            slider.ButtonTextAR = dto.ButtonTextAR;
            slider.ImageUrl = dto.ImageUrl;
            slider.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return new SliderDto
            {
                Id = slider.Id,
                TitleAR = slider.TitleAR,
                TitleEN = slider.TitleEN,
                SubtitleAR = slider.SubtitleAR,
                SubtitleEN = slider.SubtitleEN,
                ImageUrl = slider.ImageUrl,
                DescriptionEN = slider.DescriptionEN,
                DescriptionAR = slider.DescriptionAR,
                ButtonTextAR = slider.ButtonTextAR,
                ButtonTextEN = slider.ButtonTextEN,
                ButtonLink = slider.ButtonLink,
                IsActive = slider.IsActive
            };
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

