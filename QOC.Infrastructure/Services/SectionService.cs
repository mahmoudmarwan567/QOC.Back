using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs.Section;
using QOC.Domain.Entities;
using QOC.Infrastructure.Contracts;
using QOC.Infrastructure.Helpers;
using QOC.Infrastructure.Persistence;

namespace QOC.Infrastructure.Services
{
    public class SectionService : ISectionService
    {
        private readonly ApplicationDbContext _context;
        public SectionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SectionDto>> GetAllAsync()
        {
            return await _context.Sections
                .Select(s => new SectionDto
                {
                    Id = s.Id,
                    Description = CultureHelper.IsArabic() ? s.DescriptionAR : s.DescriptionEN,
                    ImagePath = s.ImagePath
                }).ToListAsync();
        }

        public async Task<SectionDto> GetByIdAsync(int id)
        {
            var section = await _context.Sections.FindAsync(id);
            if (section == null) throw new Exception("Section not found");

            return new SectionDto
            {
                Id = section.Id,
                Description = CultureHelper.IsArabic() ? section.DescriptionAR : section.DescriptionEN,
                ImagePath = section.ImagePath
            };
        }

        public async Task<SectionDto> CreateAsync(SectionRequestDto dto)
        {
            var section = new Section
            {
                DescriptionAR = dto.DescriptionAR,
                DescriptionEN = dto.DescriptionEN,
                ImagePath = dto.ImagePath
            };

            _context.Sections.Add(section);
            await _context.SaveChangesAsync();

            return new SectionDto
            {
                Id = section.Id,
                Description = CultureHelper.IsArabic() ? section.DescriptionAR : section.DescriptionEN,
                ImagePath = section.ImagePath
            };
        }

        public async Task<SectionDto> UpdateAsync(int id, SectionRequestDto dto)
        {
            var section = await _context.Sections.FindAsync(id);
            if (section == null) throw new Exception("Section not found");

            section.DescriptionAR = dto.DescriptionAR;
            section.DescriptionEN = dto.DescriptionEN;
            section.ImagePath = dto.ImagePath;

            _context.Sections.Update(section);
            await _context.SaveChangesAsync();

            return new SectionDto
            {
                Id = section.Id,
                Description = CultureHelper.IsArabic() ? section.DescriptionAR : section.DescriptionEN,
                ImagePath = section.ImagePath
            };
        }

        public async Task DeleteAsync(int id)
        {
            var section = await _context.Sections.FindAsync(id);
            if (section == null) throw new Exception("Section not found");

            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
        }
    }
}
