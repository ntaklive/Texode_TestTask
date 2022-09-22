using AutoMapper;
using TestTask.Shared.Dto;
using TestTask.Shared.Entities;

namespace TestTask.Shared.AutoMapper
{
    public class SharedMappingProfile : Profile
    {
        public SharedMappingProfile()
        {
            CreateMap<CardDto, Card>();
            CreateMap<Card, CardDto>();
        }
    }
}