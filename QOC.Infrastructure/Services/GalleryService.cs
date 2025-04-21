using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs.Gallery;
using QOC.Domain.Entities;
using QOC.Infrastructure.Contracts;
using QOC.Infrastructure.Persistence;

namespace QOC.Infrastructure.Services
{
    public class GalleryService : IGalleryService
    {
        public ApplicationDbContext _context { get; }
        public GalleryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GalleryDto>> GetAllAsync()
        {
            var galleries = await _context.Galleries.ToListAsync();
            return galleries.Select(galleries => new GalleryDto
            {
                Id = galleries.Id,
                DescriptionAR = galleries.DescriptionAR,
                DescriptionEN = galleries.DescriptionEN,
                ImagePath = galleries.ImagePath
            });
        }
        public async Task<List<GalleryDto>> BulkCreateAsync(IEnumerable<GalleryRequestDto> galleries)
        {
            if (galleries == null || !galleries.Any())
                return null;

            var galleryEntities = galleries.Select(g => new Gallery
            {
                ImagePath = g.ImagePath,
                DescriptionAR = g.DescriptionAR,
                DescriptionEN = g.DescriptionEN
            }).ToList();

            await _context.Galleries.AddRangeAsync(galleryEntities);
            await _context.SaveChangesAsync();

            var result = galleryEntities.Select(g => new GalleryDto
            {
                Id = g.Id,
                DescriptionAR = g.DescriptionAR,
                DescriptionEN = g.DescriptionEN,
                ImagePath = g.ImagePath
            }).ToList();

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            var gallery = await _context.Galleries.FindAsync(id);
            if (gallery == null) return;
            _context.Galleries.Remove(gallery);
            await _context.SaveChangesAsync();
        }

    }
}
