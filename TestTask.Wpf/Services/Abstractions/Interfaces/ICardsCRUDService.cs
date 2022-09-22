using TestTask.Shared.Entities;
using TestTask.Wpf.Dto;

namespace TestTask.Wpf.Services.Abstractions;

public interface ICardsCRUDService : ICRUDService<Card, CardForCreationDto, CardForUpdateDto>
{
    
}