using TestTask.Shared.Entities.Abstractions.Interfaces;

namespace TestTask.Wpf.Dto;

public class CardForCreationDto : IDtoForCreation
{
    public string Label { get; init; } = null!;
    
    public byte[] ImageBytes { get; init; } = null!;
}