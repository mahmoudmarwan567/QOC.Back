using AutoMapper;
using QOC.Application.DTOs;
using QOC.Application.DTOs.Project;
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


            CreateMap<Company, CompanyDto>()
            .ForMember(dest => dest.LogoPath, opt => opt.MapFrom(src => src.Logo))
            .ReverseMap()
            .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.LogoPath));

            CreateMap<CompanySocial, CompanySocialDto>()
                .ReverseMap();
        }
    }
}
