using TestTask.Wpf.Services;
using TestTask.Wpf.ViewModels.Factories.Interfaces;

namespace TestTask.Wpf.ViewModels.Factories.Implementations;

public class CardPreviewViewModelFactory : ICardPreviewViewModelFactory
{
    private readonly ApplicationState _applicationState;
    private readonly ICardCreateOrEditViewModelFactory _cardCreateOrEditViewModelFactory;
    private readonly ICardDeleteViewModelFactory _cardDeleteViewModelFactory;

    public CardPreviewViewModelFactory(
        ApplicationState applicationState,
        ICardCreateOrEditViewModelFactory cardCreateOrEditViewModelFactory,
        ICardDeleteViewModelFactory cardDeleteViewModelFactory)
    {
        _applicationState = applicationState;
        _cardCreateOrEditViewModelFactory = cardCreateOrEditViewModelFactory;
        _cardDeleteViewModelFactory = cardDeleteViewModelFactory;
    }
    
    public ICardPreviewViewModel Create(ICardViewModel cardViewModel)
    {
        return new CardPreviewViewModel(cardViewModel, _applicationState, _cardCreateOrEditViewModelFactory, _cardDeleteViewModelFactory);
    }
}