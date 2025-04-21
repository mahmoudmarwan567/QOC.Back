using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QOC.Application.DTOs.Project;
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

            var result = categories.Select(c => new ResponseProjectCategoryDto
            {
                Id = c.Id,
                NameAR = c.NameAR,
                NameEN = c.NameEN,
                DescriptionAR = c.DescriptionAR,
                DescriptionEN = c.DescriptionEN,
                ImagePath = c.ImagePath,
                Projects = c.Projects.Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    ProjectNameAR = p.ProjectNameAR,
                    ProjectNameEN = p.ProjectNameEN,
                    ProjectDescriptionAR = p.ProjectDescriptionAR,
                    ProjectDescriptionEN = p.ProjectDescriptionEN,
                    ProjectPropertiesAR = p.ProjectPropertiesAR,
                    ProjectPropertiesEN = p.ProjectPropertiesEN,
                    ProjectCategoryId = p.ProjectCategoryId,
                    ProjectImages = _mapper.Map<List<ProjectImageResponseDto>>(p.ProjectImages)
                })
            }).ToList();
            return result;
        }

        public async Task<ResponseProjectCategoryDto?> GetByIdAsync(int id)
        {
            var category = await _context.ProjectCategories
                .Include(c => c.Projects)
                    .ThenInclude(p => p.ProjectImages)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return null;


            return new ResponseProjectCategoryDto
            {
                Id = id,
                NameAR = category.NameAR,
                NameEN = category.NameEN,
                DescriptionAR = category.DescriptionAR,
                DescriptionEN = category.DescriptionEN,

                ImagePath = category.ImagePath,
                Projects = category.Projects.Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    ProjectNameAR = p.ProjectNameAR,
                    ProjectNameEN = p.ProjectNameEN,
                    ProjectDescriptionAR = p.ProjectDescriptionAR,
                    ProjectDescriptionEN = p.ProjectDescriptionEN,
                    ProjectPropertiesAR = p.ProjectPropertiesAR,
                    ProjectPropertiesEN = p.ProjectPropertiesEN,
                    ProjectCategoryId = p.ProjectCategoryId,
                    ProjectImages = _mapper.Map<List<ProjectImageResponseDto>>(p.ProjectImages)
                })
            };
        }

        public async Task<ResponseProjectCategoryDto> CreateAsync(RequestProjectCategoryDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var projectCategory = new ProjectCategory
            {
                NameAR = dto.NameAR,
                DescriptionAR = dto.DescriptionAR,
                NameEN = dto.NameEN,
                DescriptionEN = dto.DescriptionEN,
                ImagePath = dto.ImagePath
            };
            _context.ProjectCategories.Add(projectCategory);
            await _context.SaveChangesAsync();

            return new ResponseProjectCategoryDto
            {
                Id = projectCategory.Id,
                NameAR = dto.NameAR,
                NameEN = dto.NameEN,
                DescriptionAR = dto.DescriptionAR,
                DescriptionEN = dto.DescriptionEN,
                ImagePath = projectCategory.ImagePath
            };
        }

        public async Task<ResponseProjectCategoryDto?> UpdateAsync(int id, RequestProjectCategoryDto dto)
        {
            var category = await _context.ProjectCategories.FindAsync(id);

            if (category == null) return null;

            _mapper.Map(dto, category);
            await _context.SaveChangesAsync();
            var projects = await _context.Projects
                .Include(p => p.ProjectImages)
                .Where(p => p.ProjectCategoryId == id)
                .ToListAsync();
            return new ResponseProjectCategoryDto
            {
                Id = id,
                NameAR = category.NameAR,
                NameEN = category.NameEN,
                DescriptionAR = category.DescriptionAR,
                DescriptionEN = category.DescriptionEN,
                ImagePath = category.ImagePath,
                Projects = projects.Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    ProjectNameAR = p.ProjectNameAR,
                    ProjectNameEN = p.ProjectNameEN,
                    ProjectDescriptionAR = p.ProjectDescriptionAR,
                    ProjectDescriptionEN = p.ProjectDescriptionEN,
                    ProjectPropertiesAR = p.ProjectPropertiesAR,
                    ProjectPropertiesEN = p.ProjectPropertiesEN,
                    ProjectCategoryId = p.ProjectCategoryId,
                    ProjectImages = _mapper.Map<List<ProjectImageResponseDto>>(p.ProjectImages)
                })
            };
        }

        public async Task<string> DeleteAsync(int id)
        {
            var category = await _context.ProjectCategories
                .Include(c => c.Projects)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return "Category not found.";

            if (category.Projects != null && category.Projects.Any())
                return "Cannot delete. There are related projects.";

            _context.ProjectCategories.Remove(category);
            await _context.SaveChangesAsync();

            return "Category deleted successfully.";
        }

        public async Task<List<ResponseBasicProjectCategoryDto>> GetAllWithoutProjectsAsync()
        {
            var categories = await _context.ProjectCategories.ToListAsync();
            return categories.Select(c => new ResponseBasicProjectCategoryDto
            {
                Id = c.Id,
                NameAR = c.NameAR,
                NameEN = c.NameEN,
            }).ToList();
        }
    }
}
