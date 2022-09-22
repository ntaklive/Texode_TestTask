using System.Windows.Input;
using TestTask.Wpf.Helpers;
using TestTask.Wpf.Services;
using TestTask.Wpf.Services.Abstractions;

namespace TestTask.Wpf.ViewModels;

public class CardDeleteViewModel : ViewModelBase, ICardDeleteViewModel
{
    private readonly ApplicationState _applicationState;
    private readonly ICardViewModel _cardViewModel;
    private readonly ICardsCRUDService _cardsCRUDService;

    public CardDeleteViewModel()
    {
        _cardViewModel = null!;
        _cardsCRUDService = null!;
        _applicationState = null!;

        DeleteCardCommand = new RelayCommand(_ => { }, _ => true);
        CancelCommand = new RelayCommand(_ => { }, _ => true);
    }
    
    public CardDeleteViewModel(
        ApplicationState applicationState,
        ICardViewModel cardViewModel,
        ICardsCRUDService cardsCRUDService)
    {
        _applicationState = applicationState;
        _cardViewModel = cardViewModel;
        _cardsCRUDService = cardsCRUDService;
        
        DeleteCardCommand = CreateDeleteCardCommand();
        CancelCommand = CreateCancelCommand();
    }

    public ICommand DeleteCardCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    private RelayCommand CreateDeleteCardCommand()
    {
        return new RelayCommand(async _ =>
            {
                _applicationState.ShowPopup(new InfoViewModel("Deleting... Please wait patient."));
                await _cardsCRUDService.DeleteAsync(_cardViewModel.Id);
                await _applicationState.RefreshCardsAsync();
                
                _applicationState.ClosePopup();
                _applicationState.CurrentViewModel = new InfoViewModel("The selected card was successful deleted.");
            },
            _ => true);
    }   
    
    private RelayCommand CreateCancelCommand()
    {
        return new RelayCommand(_ => _applicationState.ClosePopup(),
            _ => true);
    }
}