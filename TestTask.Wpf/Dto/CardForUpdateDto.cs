using TestTask.Shared.Entities.Abstractions.Interfaces;

namespace TestTask.Wpf.Dto;

public class CardForUpdateDto : IDtoForUpdate
{
    public string Label { get; set; } = null!;
    
    public byte[]? ImageBytes { get; set; }
}