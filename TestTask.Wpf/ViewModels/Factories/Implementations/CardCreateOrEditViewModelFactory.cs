using Microsoft.Extensions.Logging;
using TestTask.Wpf.Services;
using TestTask.Wpf.Services.Abstractions;
using TestTask.Wpf.ViewModels.Factories.Interfaces;

namespace TestTask.Wpf.ViewModels.Factories.Implementations;

public class CardCreateOrEditViewModelFactory : ICardCreateOrEditViewModelFactory
{
    private readonly ApplicationState _applicationState;
    private readonly ISelectFileDialogService _selectFileDialogService;
    private readonly ICardsCRUDService _cardsCRUDService;
    private readonly ILogger<CardCreateOrEditViewModel> _logger;

    public CardCreateOrEditViewModelFactory(
        ApplicationState applicationState,
        ISelectFileDialogService selectFileDialogService,
        ICardsCRUDService cardsCRUDService,
        ILogger<CardCreateOrEditViewModel> logger)
    {
        _applicationState = applicationState;
        _selectFileDialogService = selectFileDialogService;
        _cardsCRUDService = cardsCRUDService;
        _logger = logger;
    }
    
    public ICardCreateOrEditViewModel CreateCardCreateViewModel()
    {
        return new CardCreateOrEditViewModel(null, _applicationState, _selectFileDialogService, _cardsCRUDService, _logger);
    }
    
    public ICardCreateOrEditViewModel CreateCardEditViewModel(ICardViewModel cardViewModel)
    {
        return new CardCreateOrEditViewModel(cardViewModel, _applicationState, _selectFileDialogService, _cardsCRUDService, _logger);
    }
}