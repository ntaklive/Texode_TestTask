using System.Windows.Input;

namespace TestTask.Wpf.ViewModels;

public interface ICardDeleteViewModel : IViewModel
{
    public ICommand DeleteCardCommand { get; set; }
    public ICommand CancelCommand { get; set; }
}