using QOC.Application.DTOs.Project;

namespace QOC.Infrastructure.Contracts
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync();
        Task<ProjectResponseDto?> GetProjectByIdAsync(int id);
        Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto projectDto);
        Task<ProjectResponseDto> UpdateProjectAsync(ProjectUpdateDto projectDto);
        Task<bool> DeleteProjectAsync(int id);
    }
}
