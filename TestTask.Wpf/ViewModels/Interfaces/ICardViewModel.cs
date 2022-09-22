namespace TestTask.Wpf.ViewModels;

public interface ICardViewModel : IViewModel
{
    public int Id { get; }
    public string Label { get; set; }
    public string? ImageUrl { get; set; }
}