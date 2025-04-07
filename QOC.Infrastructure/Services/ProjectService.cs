using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
                if (imageDto.ImageFile != null)
                {
                    var filePath = await SaveImageAsync(imageDto.ImageFile);

                    var projectImage = new ProjectImage { ImagePath = filePath };
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
            var existingImagePaths = project.ProjectImages.Select(pi => pi.ImagePath).ToList();
            var newImages = new List<ProjectImage>();
            var imagesToDelete = new List<ProjectImage>();

            foreach (var imageDto in projectDto.ProjectImages)
            {
                if (imageDto.Image != null)
                {
                    var filePath = await SaveImageAsync(imageDto.Image);
                    newImages.Add(new ProjectImage { ImagePath = filePath });
                }
            }

            foreach (var existingImage in project.ProjectImages)
            {
                if (!projectDto.ProjectImages.Any(pi => pi.Image == null && pi.ImagePath == existingImage.ImagePath))
                {
                    imagesToDelete.Add(existingImage);
                }
            }

            foreach (var image in imagesToDelete)
            {
                DeleteImage(image.ImagePath);
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
                DeleteImage(image.ImagePath);
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            string uploadsFolder = Path.Combine(_env.WebRootPath, "assets/images");
            Directory.CreateDirectory(uploadsFolder);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/assets/images/{fileName}";
        }

        private void DeleteImage(string? imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                string fullPath = Path.Combine(_env.WebRootPath, imagePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
        }
    }
}
