using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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

        public ProjectService(ApplicationDbContext context, IWebHostEnvironment env, IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
            var projects = await _context.Projects.Include(p => p.ProjectImages).ToListAsync();
            return _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
        }

        public async Task<ProjectResponseDto?> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects.Include(p => p.ProjectImages).FirstOrDefaultAsync(p => p.Id == id);
            return project == null ? null : _mapper.Map<ProjectResponseDto>(project);
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto projectDto)
        {
            if (projectDto == null) throw new ArgumentNullException(nameof(projectDto));

            var project = new Project
            {
                ProjectName = projectDto.ProjectName,
                ProjectDescription = projectDto.ProjectDescription
            };
            var projectImages = new List<ProjectImage>();

            foreach (var imageDto in projectDto.ProjectImages)
            {
                if (imageDto.ImagePath != null)
                {
                    var projectImage = new ProjectImage { ImagePath = imageDto.ImagePath };
                    projectImages.Add(projectImage);
                }
            }

            project.ProjectImages = projectImages;

            // Save to database
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectResponseDto>(project);
        }
        public async Task<ProjectResponseDto?> UpdateProjectAsync(ProjectUpdateDto projectDto)
        {
            if (projectDto == null) throw new ArgumentNullException(nameof(projectDto));

            var project = await _context.Projects.Include(p => p.ProjectImages).FirstOrDefaultAsync(p => p.Id == projectDto.Id);
            if (project == null) return null;

            project.ProjectName = projectDto.ProjectName;
            project.ProjectDescription = projectDto.ProjectDescription;

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

            return _mapper.Map<ProjectResponseDto>(project);
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
            return true;
        }
    }
}
