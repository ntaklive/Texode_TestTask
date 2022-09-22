using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TestTask.Shared.Entities;
using TestTask.Shared.Extensions;
using TestTask.Wpf.Enums;
using TestTask.Wpf.Helpers;
using TestTask.Wpf.Services;
using TestTask.Wpf.ViewModels.Factories.Interfaces;

namespace TestTask.Wpf.ViewModels;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private readonly ApplicationState _applicationState;
    private readonly ICardPreviewViewModelFactory _cardPreviewViewModelFactory;
    private readonly ICardCreateOrEditViewModelFactory _cardCreateOrEditViewModelFactory;
    private readonly ICardViewModelFactory _cardViewModelFactory;
    private SortType _selectedSortType;

    public MainWindowViewModel()
    {
        Cards = new ObservableCollection<ICardViewModel>();
        
        _applicationState = null!;
        _cardPreviewViewModelFactory = null!;
        _cardViewModelFactory = null!;
        _cardCreateOrEditViewModelFactory = null!;
        
        ShowCardPreviewViewCommand = new RelayCommand(_ => { }, _ => true);
        ShowCreateCardViewCommand = new RelayCommand(_ => { }, _ => true);

        SortCardsByCommand = new RelayCommand(_ => { }, _ => true);
    }

    public MainWindowViewModel(
        ApplicationState applicationState,
        ICardViewModelFactory cardViewModelFactory,
        ICardPreviewViewModelFactory cardPreviewViewModelFactory,
        ICardCreateOrEditViewModelFactory cardCreateOrEditViewModelFactory)
    {
        _applicationState = applicationState;
        _cardViewModelFactory = cardViewModelFactory;
        _cardPreviewViewModelFactory = cardPreviewViewModelFactory;
        _cardCreateOrEditViewModelFactory = cardCreateOrEditViewModelFactory;

        Cards = new ObservableCollection<ICardViewModel>();

        ShowCardPreviewViewCommand = CreateShowCardPreviewViewCommand();
        ShowCreateCardViewCommand = CreateShowCreateCardViewCommand();
        SortCardsByCommand = CreateSortCardsByCommand();

        WeakEventManager<ObservableCollection<Card>, EventArgs>.AddHandler(
            _applicationState.Cards,
            nameof(_applicationState.Cards.CollectionChanged),
            OnApplicationStateCardsCollectionChanged);

        WeakEventManager<ApplicationState, PropertyChangedEventArgs>.AddHandler(
            _applicationState,
            nameof(_applicationState.PropertyChanged),
            OnApplicationStatePropertyChanged);
    }

    public bool IsPopupShown => _applicationState.IsPopupShown;
    
    public IViewModel CurrentViewModel => _applicationState.CurrentViewModel;
    
    public IViewModel? CurrentPopupViewModel => _applicationState.CurrentPopupViewModel;

    public ObservableCollection<ICardViewModel> Cards { get; set; }

    public SortType SelectedSortType
    {
        get => _selectedSortType;
        set
        {
            _selectedSortType = value;

            SortCardsByCommand.Execute(value);

            OnPropertyChanged(nameof(SelectedSortType));
        }
    }

    public ICommand ShowCardPreviewViewCommand { get; set; }
    public ICommand ShowCreateCardViewCommand { get; set; }
    public ICommand SortCardsByCommand { get; set; }

    private RelayCommand CreateShowCardPreviewViewCommand()
    {
        return new RelayCommand(arg => _applicationState.CurrentViewModel =
                _cardPreviewViewModelFactory.Create((arg as ICardViewModel)!),
            _ => true);
    }

    private RelayCommand CreateShowCreateCardViewCommand()
    {
        return new RelayCommand(_ => _applicationState.CurrentViewModel =
                _cardCreateOrEditViewModelFactory.CreateCardCreateViewModel(),
            _ => true);
    }

    private RelayCommand CreateSortCardsByCommand()
    {
        return new RelayCommand(arg =>
        {
            SortType sortType = arg is SortType type ? type : throw new ArgumentException("Invalid enum type");

            ShowHomeView();
            Cards.Clear();

            if (sortType == SortType.AToZ)
            {
                _applicationState.Cards
                    .OrderBy(card => card.Label)
                    .Select(_cardViewModelFactory.Create)
                    .ForEach(Cards.Add);
            }
            else if (sortType == SortType.ZToA)
            {
                _applicationState.Cards
                    .OrderByDescending(card => card.Label)
                    .Select(_cardViewModelFactory.Create)
                    .ForEach(Cards.Add);
            }
        }, _ => true);
    }

    private void ShowHomeView()
    {
        _applicationState.CurrentViewModel = new HomeViewModel();
    }

    private void OnApplicationStatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(e.PropertyName);
    }

    private void OnApplicationStateCardsCollectionChanged(object? sender, EventArgs e)
    {
        Cards.Clear();
        _applicationState.Cards.Select(_cardViewModelFactory.Create).ForEach(Cards.Add);
        SelectedSortType = SortType.AToZ;
    }
}