using System.Windows.Input;
using TestTask.Wpf.Helpers;
using TestTask.Wpf.Services;
using TestTask.Wpf.ViewModels.Factories.Interfaces;

namespace TestTask.Wpf.ViewModels;

public class CardPreviewViewModel : ViewModelBase, ICardPreviewViewModel
{
    private readonly ApplicationState _applicationState;
    private readonly ICardCreateOrEditViewModelFactory _cardCreateOrEditViewModelFactory;
    private readonly ICardDeleteViewModelFactory _cardDeleteViewModelFactory;

    public CardPreviewViewModel()
    {
        _applicationState = null!;
        _cardCreateOrEditViewModelFactory = null!;
        _cardDeleteViewModelFactory = null!;
        
        CardViewModel = null!;

        ShowEditCardViewCommand = new RelayCommand(_ => { }, _ => true);
        ShowDeleteCardViewCommand = new RelayCommand(_ => { }, _ => true);
    }

    public CardPreviewViewModel(ICardViewModel cardCardViewModel,
        ApplicationState applicationState,
        ICardCreateOrEditViewModelFactory cardCreateOrEditViewModelFactory,
        ICardDeleteViewModelFactory cardDeleteViewModelFactory)
    {
        _applicationState = applicationState;
        _cardCreateOrEditViewModelFactory = cardCreateOrEditViewModelFactory;
        _cardDeleteViewModelFactory = cardDeleteViewModelFactory;
        
        CardViewModel = cardCardViewModel;
        
        ShowEditCardViewCommand = CreateShowEditCardViewCommand();
        ShowDeleteCardViewCommand = CreateShowDeleteCardViewCommand();
    }
    
    public ICardViewModel CardViewModel { get; }
    
    public ICommand ShowEditCardViewCommand { get; set; }
    public ICommand ShowDeleteCardViewCommand { get; set; }
    
    private RelayCommand CreateShowEditCardViewCommand()
    {
        return new RelayCommand(_ => _applicationState.CurrentViewModel =
                _cardCreateOrEditViewModelFactory.CreateCardEditViewModel(CardViewModel),
            _ => true);
    }    
    
    private RelayCommand CreateShowDeleteCardViewCommand()
    {
        return new RelayCommand(_ => _applicationState.ShowPopup(
                _cardDeleteViewModelFactory.Create(CardViewModel)),
            _ => true);
    }
}