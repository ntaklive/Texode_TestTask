using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestTask.Shared.Entities;
using TestTask.Shared;
using TestTask.Shared.Extensions;
using TestTask.Wpf.Services.Abstractions;
using TestTask.Wpf.ViewModels;

namespace TestTask.Wpf.Services;

public sealed class ApplicationState : INotifyPropertyChanged
{
    private readonly ICardsCRUDService _cardsCRUDService;
    private readonly ILogger<ApplicationState> _logger;
    private bool _isPopupShown;

    private IViewModel _currentViewModel;
    private IViewModel? _currentPopupViewModel;

    public ApplicationState(
        ICardsCRUDService cardsCRUDService,
        ILogger<ApplicationState> logger)
    {
        _cardsCRUDService = cardsCRUDService;
        _logger = logger;
        _currentViewModel = new HomeViewModel();
        _currentPopupViewModel = null!;
        _isPopupShown = false;
        
        Cards = new ObservableCollection<Card>();
    }

    public ObservableCollection<Card> Cards { get; set; }

    public bool IsPopupShown
    {
        get => _isPopupShown;
        set
        {
            _isPopupShown = value;
            
            OnPropertyChanged(nameof(IsPopupShown));
        }
    }
    
    public IViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }   
    
    public IViewModel? CurrentPopupViewModel
    {
        get => _currentPopupViewModel;
        set
        {
            _currentPopupViewModel = value;
            
            OnPropertyChanged(nameof(CurrentPopupViewModel));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task RefreshCardsAsync()
    {
        try
        {
            IEnumerable<Card> cards = await _cardsCRUDService.GetAllAsync();
            
            Cards.Clear();
            cards.ForEach(Cards.Add);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "It is unable to load the available cards from the remote source");
            await ShowPopupAsync(new InfoViewModel("It is unable to load the available cards from the remote source"),
                2);
        }
    }

    public void ShowPopup(IViewModel viewModel)
    {
        CurrentPopupViewModel = viewModel;
        IsPopupShown = true;
    }
    
    public async Task ShowPopupAsync(IViewModel viewModel, int delaySeconds)
    {
        ShowPopup(viewModel);
        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
        ClosePopup();
    }

    public void ClosePopup()
    {
        CurrentPopupViewModel = null;
        IsPopupShown = false;
    }

    public async Task ClosePopupAsync(int delaySeconds)
    {
        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
        ClosePopup();
    }
    
    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}