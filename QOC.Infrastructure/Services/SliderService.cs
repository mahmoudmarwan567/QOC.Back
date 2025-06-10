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
        private static IEnumerable<SliderDto>? _cachedSliders;
        private static DateTime _lastCacheTime;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);
        private static readonly object _cacheLock = new();

        public SliderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SliderDto>> GetAllSlidersAsync()
        {
            lock (_cacheLock)
            {
                if (_cachedSliders != null && DateTime.Now - _lastCacheTime < CacheDuration)
                {
                    return _cachedSliders;
                }
            }

            var sliders = await _context.Sliders.AsNoTracking().ToListAsync();
            var result = sliders.Select(s => new SliderDto
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

            lock (_cacheLock)
            {
                _cachedSliders = result;
                _lastCacheTime = DateTime.Now;
            }

            return result;
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
            InvalidateCache();

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

            await _context.SaveChangesAsync();
            InvalidateCache();

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
            InvalidateCache();
        }

        private void InvalidateCache()
        {
            lock (_cacheLock)
            {
                _cachedSliders = null;
                _lastCacheTime = DateTime.MinValue;
            }
        }
    }
}
