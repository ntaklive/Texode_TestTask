using System.Windows.Input;

namespace TestTask.Wpf.ViewModels;

public interface ICardPreviewViewModel : IViewModel
{
    public ICommand ShowEditCardViewCommand { get; set; }
    public ICommand ShowDeleteCardViewCommand { get; set; }
}