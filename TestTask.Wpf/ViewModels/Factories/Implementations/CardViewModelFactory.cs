using TestTask.Shared.Entities;
using TestTask.Wpf.ViewModels.Factories.Interfaces;

namespace TestTask.Wpf.ViewModels.Factories.Implementations;

public class CardViewModelFactory : ICardViewModelFactory
{
    public ICardViewModel Create(Card card)
    {
        return new CardViewModel(card);
    }
}