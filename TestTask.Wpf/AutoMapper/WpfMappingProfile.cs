using AutoMapper;
using TestTask.Shared.Entities;
using TestTask.Wpf.Dto;

namespace TestTask.Wpf.AutoMapper
{
    public class WpfMappingProfile : Profile
    {
        public WpfMappingProfile()
        {
            CreateMap<CardForCreationDto, Card>()                
                .ForMember(dto => dto.ImageUrl, opt => opt.Ignore());
            CreateMap<CardForUpdateDto, Card>()
                .ForMember(dto => dto.ImageUrl, opt => opt.Ignore());
        }
    }
}