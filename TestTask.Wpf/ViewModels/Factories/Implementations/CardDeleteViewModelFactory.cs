using TestTask.Wpf.Services;
using TestTask.Wpf.Services.Abstractions;
using TestTask.Wpf.ViewModels.Factories.Interfaces;

namespace TestTask.Wpf.ViewModels.Factories.Implementations;

public class CardDeleteViewModelFactory : ICardDeleteViewModelFactory
{
    private readonly ApplicationState _applicationState;
    private readonly ICardsCRUDService _cardsCRUDService;

    public CardDeleteViewModelFactory(ApplicationState applicationState, ICardsCRUDService cardsCRUDService)
    {
        _applicationState = applicationState;
        _cardsCRUDService = cardsCRUDService;
    }
    
    public ICardDeleteViewModel Create(ICardViewModel cardViewModel)
    {
        return new CardDeleteViewModel(_applicationState, cardViewModel, _cardsCRUDService);
    }
}