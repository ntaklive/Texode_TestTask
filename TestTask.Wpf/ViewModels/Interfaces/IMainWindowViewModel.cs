using System.Collections.ObjectModel;
using System.Windows.Input;
using TestTask.Wpf.Enums;

namespace TestTask.Wpf.ViewModels;

public interface IMainWindowViewModel : IViewModel
{
    public bool IsPopupShown { get;  }

    public IViewModel CurrentViewModel { get; }
    
    public IViewModel? CurrentPopupViewModel { get; }

    public ObservableCollection<ICardViewModel> Cards { get; set; }
    public SortType SelectedSortType { get; set; }

    public ICommand ShowCardPreviewViewCommand { get; set; }
    public ICommand ShowCreateCardViewCommand { get; set; }
    public ICommand SortCardsByCommand { get; set; }
}