using System.Windows.Input;

namespace TestTask.Wpf.ViewModels;

public interface ICardCreateOrEditViewModel : IViewModel
{
    public string Label { get; set; }
    public string? ImageFilepath { get; set; }
    
    public ICommand SelectImageCommand { get; set; }
    public ICommand CreateOrEditCardCommand { get; set; }
}