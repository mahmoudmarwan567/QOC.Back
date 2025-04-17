using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs.ProjectCategory;
using QOC.Domain.Entities;
using QOC.Infrastructure.Contracts;
using QOC.Infrastructure.Persistence;

namespace QOC.Infrastructure.Services
{
    public class ProjectCategoryService : IProjectCategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectCategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ResponseProjectCategoryDto>> GetAllAsync()
        {
            var categories = await _context.ProjectCategories
                .Include(c => c.Projects)
                    .ThenInclude(p => p.ProjectImages)
                .ToListAsync();

            return categories.Select(category => new ResponseProjectCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Project = category.Projects.Select(p => new ResponseSingleProjectDto
                {
                    Id = p.Id,
                    ImagePath = p.ProjectImages.FirstOrDefault()?.ImagePath
                }).FirstOrDefault()
            }).ToList();
        }

        public async Task<ResponseProjectCategoryDto?> GetByIdAsync(int id)
        {
            var category = await _context.ProjectCategories
                .Include(c => c.Projects)
                    .ThenInclude(p => p.ProjectImages)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return null;

            var project = category.Projects.Select(p => new ResponseSingleProjectDto
            {
                Id = p.Id,
                ImagePath = p.ProjectImages.FirstOrDefault()?.ImagePath
            }).FirstOrDefault();

            return new ResponseProjectCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Project = project
            };
        }

        public async Task<ResponseProjectCategoryDto> CreateAsync(RequestProjectCategoryDto dto)
        {
            var entity = _mapper.Map<ProjectCategory>(dto);

            _context.ProjectCategories.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<ResponseProjectCategoryDto>(entity);
        }

        public async Task<ResponseProjectCategoryDto?> UpdateAsync(int id, RequestProjectCategoryDto dto)
        {
            var category = await _context.ProjectCategories.FindAsync(id);

            if (category == null) return null;

            _mapper.Map(dto, category);
            await _context.SaveChangesAsync();

            return _mapper.Map<ResponseProjectCategoryDto>(category);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.ProjectCategories.FindAsync(id);
            if (category == null) return;

            _context.ProjectCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ResponseBasicProjectCategoryDto>> GetAllWithoutProjectsAsync()
        {
            var categories = await _context.ProjectCategories.ToListAsync();
            return _mapper.Map<List<ResponseBasicProjectCategoryDto>>(categories);
        }
    }
}
