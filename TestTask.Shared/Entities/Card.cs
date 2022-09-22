using TestTask.Shared.Entities.Abstractions.Interfaces;

namespace TestTask.Shared.Entities;

public class Card : IHasKey
{
    public int Id { get; set; }
    public string Label { get; set; } = null!;
    public string? ImageUrl { get; set; }
}