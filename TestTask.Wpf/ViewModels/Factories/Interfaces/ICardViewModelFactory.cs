using TestTask.Shared.Entities;

namespace TestTask.Wpf.ViewModels.Factories.Interfaces;

public interface ICardViewModelFactory
{
    public ICardViewModel Create(Card card);
}