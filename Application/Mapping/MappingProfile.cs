using Application.DTO;
using AutoMapper;
using TaskManager.Domain.Models;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<CreateWorkTaskDto, WorkTask>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore());

            CreateMap<WorkTask, WorkTaskDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Без категории"));

            CreateMap<UpdateWorkTaskDto, WorkTask>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore());

            CreateMap<CreateTagDto, Tag>();
            CreateMap<Tag, TagDto>();
        }
    }
}
