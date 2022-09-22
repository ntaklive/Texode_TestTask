using TestTask.Shared.Entities.Abstractions.Interfaces;

namespace TestTask.Shared.Dto;

public class CardDto : IDto
{
    public int Id { get; set; }
    public string Label { get; set; } = null!;
    public string? ImageUrl { get; set; }
}