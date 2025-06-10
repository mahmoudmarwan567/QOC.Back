using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using QOC.Application.DTOs.Project;
using QOC.Domain.Entities.Project;
using QOC.Infrastructure.Contracts;
using QOC.Infrastructure.Persistence;

namespace QOC.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        private const string AllProjectsCacheKey = "all_projects";

        public ProjectService(ApplicationDbContext context, IWebHostEnvironment env, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
            if (!_cache.TryGetValue(AllProjectsCacheKey, out IEnumerable<ProjectResponseDto> cachedProjects))
            {
                var projects = await _context.Projects.Include(p => p.ProjectImages).ToListAsync();
                cachedProjects = projects.Select(p => new ProjectResponseDto
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
                }).ToList();

                _cache.Set(AllProjectsCacheKey, cachedProjects, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }

            return cachedProjects!;
        }

        public async Task<ProjectResponseDto?> GetProjectByIdAsync(int id)
        {
            var all = await GetAllProjectsAsync(); // استفادة من الكاش
            return all.FirstOrDefault(p => p.Id == id);
        }




        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto projectDto)
        {
            if (projectDto == null) throw new ArgumentNullException(nameof(projectDto));

            var project = new Project
            {
                ProjectNameAR = projectDto.ProjectNameAR,
                ProjectNameEN = projectDto.ProjectNameEN,
                ProjectDescriptionAR = projectDto.ProjectDescriptionAR,
                ProjectDescriptionEN = projectDto.ProjectDescriptionEN,
                ProjectPropertiesAR = projectDto.ProjectPropertiesAR,
                ProjectPropertiesEN = projectDto.ProjectPropertiesEN,
                ProjectCategoryId = projectDto.ProjectCategoryId,
            };

            var projectImages = new List<ProjectImage>();
            foreach (var imageDto in projectDto.ProjectImages)
            {
                if (!string.IsNullOrEmpty(imageDto.ImagePath))
                {
                    projectImages.Add(new ProjectImage { ImagePath = imageDto.ImagePath });
                }
            }

            project.ProjectImages = projectImages;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Invalidate the cache
            _cache.Remove(AllProjectsCacheKey);

            return new ProjectResponseDto
            {
                Id = project.Id,
                ProjectNameAR = project.ProjectNameAR,
                ProjectNameEN = project.ProjectNameEN,
                ProjectDescriptionAR = project.ProjectDescriptionAR,
                ProjectDescriptionEN = project.ProjectDescriptionEN,
                ProjectPropertiesAR = project.ProjectPropertiesAR,
                ProjectPropertiesEN = project.ProjectPropertiesEN,
                ProjectCategoryId = project.ProjectCategoryId,
                ProjectImages = _mapper.Map<List<ProjectImageResponseDto>>(project.ProjectImages)
            };
        }

        public async Task<ProjectResponseDto?> UpdateProjectAsync(ProjectUpdateDto projectDto)
        {
            if (projectDto == null) throw new ArgumentNullException(nameof(projectDto));

            var project = await _context.Projects.Include(p => p.ProjectImages).FirstOrDefaultAsync(p => p.Id == projectDto.Id);
            if (project == null) return null;

            project.ProjectNameAR = projectDto.ProjectNameAR;
            project.ProjectNameEN = projectDto.ProjectNameEN;
            project.ProjectDescriptionAR = projectDto.ProjectDescriptionAR;
            project.ProjectDescriptionEN = projectDto.ProjectDescriptionEN;
            project.ProjectPropertiesAR = projectDto.ProjectPropertiesAR;
            project.ProjectPropertiesEN = projectDto.ProjectPropertiesEN;
            project.ProjectCategoryId = projectDto.ProjectCategoryId;

            // Handle image updates
            var newImages = new List<ProjectImage>();
            var imagesToDelete = new List<ProjectImage>();

            foreach (var imageDto in projectDto.ProjectImages)
            {
                if (!string.IsNullOrEmpty(imageDto.ImagePath))
                {
                    // Add new images
                    if (imageDto.Id == 0)
                    {
                        newImages.Add(new ProjectImage { ImagePath = imageDto.ImagePath });
                    }
                    else
                    {
                        // Update existing images if needed
                        var existingImage = project.ProjectImages.FirstOrDefault(pi => pi.Id == imageDto.Id);
                        if (existingImage != null && existingImage.ImagePath != imageDto.ImagePath)
                        {
                            existingImage.ImagePath = imageDto.ImagePath;
                        }
                    }
                }
            }

            foreach (var existingImage in project.ProjectImages)
            {
                if (!projectDto.ProjectImages.Any(pi => pi.Id == existingImage.Id))
                {
                    imagesToDelete.Add(existingImage);
                }
            }

            foreach (var image in imagesToDelete)
            {
                project.ProjectImages.Remove(image);
            }

            foreach (var newImage in newImages)
            {
                project.ProjectImages.Add(newImage);
            }

            // Save to database
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            _cache.Remove(AllProjectsCacheKey);
            return new ProjectResponseDto
            {
                Id = project.Id,
                ProjectNameAR = project.ProjectNameAR,
                ProjectNameEN = project.ProjectNameEN,
                ProjectDescriptionAR = project.ProjectDescriptionAR,
                ProjectDescriptionEN = project.ProjectDescriptionEN,
                ProjectPropertiesAR = project.ProjectPropertiesAR,
                ProjectPropertiesEN = project.ProjectPropertiesEN,
                ProjectCategoryId = project.ProjectCategoryId,
                ProjectImages = _mapper.Map<List<ProjectImageResponseDto>>(project.ProjectImages)
            };
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.Include(p => p.ProjectImages).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null) return false;

            foreach (var image in project.ProjectImages)
            {
                _context.ProjectImages.Remove(image);
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            _cache.Remove(AllProjectsCacheKey);
            return true;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetProjectsByCategoryAsync(int categoryId)
        {
            var projects = await _context.Projects
                .Where(p => p.ProjectCategoryId == categoryId)
                .Include(p => p.ProjectImages)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetProjectsByCategoryPagedAsync(int categoryId, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            var query = _context.Projects
                .Where(p => p.ProjectCategoryId == categoryId)
                .OrderByDescending(p => p.ProjectCategoryId)
                .Skip(skip)
                .Take(pageSize)
                .Include(p => p.ProjectImages);

            var projects = await query.ToListAsync();

            return _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
        }
    }
}
