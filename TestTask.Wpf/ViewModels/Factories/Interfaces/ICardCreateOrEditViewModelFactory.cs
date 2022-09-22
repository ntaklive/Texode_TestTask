namespace TestTask.Wpf.ViewModels.Factories.Interfaces;

public interface ICardCreateOrEditViewModelFactory
{
    public ICardCreateOrEditViewModel CreateCardCreateViewModel();

    public ICardCreateOrEditViewModel CreateCardEditViewModel(ICardViewModel cardViewModel);
}