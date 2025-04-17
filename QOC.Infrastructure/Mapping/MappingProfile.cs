using AutoMapper;
using QOC.Application.DTOs.Project;
using QOC.Application.DTOs.ProjectCategory;
using QOC.Domain.Entities;
using QOC.Domain.Entities.Project;

namespace QOC.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectResponseDto>()
            .ForMember(dest => dest.ProjectImages, opt => opt.MapFrom(src => src.ProjectImages));
            CreateMap<ProjectImage, ProjectImageResponseDto>();
            CreateMap<ProjectUpdateDto, Project>()
                .ForMember(dest => dest.ProjectImages, opt => opt.Ignore());


            CreateMap<ProjectCategory, ResponseProjectCategoryDto>()
                .ForMember(dest => dest.Project, opt => opt.Ignore());

            CreateMap<RequestProjectCategoryDto, ProjectCategory>();
        }
    }
}
