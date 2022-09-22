using System;
using System.IO;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using TestTask.Wpf.Dto;
using TestTask.Wpf.Enums;
using TestTask.Wpf.Helpers;
using TestTask.Wpf.Services;
using TestTask.Wpf.Services.Abstractions;

namespace TestTask.Wpf.ViewModels;

public class CardCreateOrEditViewModel : ViewModelBase, ICardCreateOrEditViewModel
{
    private readonly ICardViewModel? _cardForEditingViewModel;
    private readonly ApplicationState _applicationState;
    private readonly ISelectFileDialogService _fileDialogService;
    private readonly ICardsCRUDService _cardsCRUDService;
    private readonly ILogger<CardCreateOrEditViewModel> _logger;
    private string _label;
    private string? _imageFilepath;
    private string _actionTypeName;

    public CardCreateOrEditViewModel()
    {
        _cardForEditingViewModel = null;
        _applicationState = null!;
        _fileDialogService = null!;
        _cardsCRUDService = null!;
        _logger = null!;

        _label = string.Empty;
        _imageFilepath = string.Empty;
        _actionTypeName = string.Empty;
        
        SelectImageCommand = new RelayCommand(_ => { }, _ => true);
        CreateOrEditCardCommand = new RelayCommand(_ => { }, _ => true);
    }

    public CardCreateOrEditViewModel(
        ICardViewModel? cardForEditingViewModel,
        ApplicationState applicationState,
        ISelectFileDialogService fileDialogService,
        ICardsCRUDService cardsCRUDService,
        ILogger<CardCreateOrEditViewModel> logger)
    {
        _cardForEditingViewModel = cardForEditingViewModel;
        _applicationState = applicationState;
        _fileDialogService = fileDialogService;
        _cardsCRUDService = cardsCRUDService;
        _logger = logger;

        _label = cardForEditingViewModel == null ? string.Empty : cardForEditingViewModel.Label;
        _imageFilepath = cardForEditingViewModel == null ? string.Empty : cardForEditingViewModel.ImageUrl;
        _actionTypeName = cardForEditingViewModel == null ? "Create" : "Apply";
        
        SelectImageCommand = CreateSelectImageCommand();
        CreateOrEditCardCommand = CreateCreateOrEditCardCommand();
    }

    public string Label
    {
        get => _label;
        set
        {
            _label = value;

            OnPropertyChanged(nameof(Label));
        }
    }

    public string? ImageFilepath
    {
        get => _imageFilepath;
        set
        {
            _imageFilepath = value;

            OnPropertyChanged(nameof(ImageFilepath));
        }
    }

    public string ActionTypeName
    {
        get => _actionTypeName;
        set
        {
            _actionTypeName = value;

            OnPropertyChanged(nameof(ActionTypeName));
        }
    }

    public ICommand SelectImageCommand { get; set; }
    public ICommand CreateOrEditCardCommand { get; set; }

    private RelayCommand CreateSelectImageCommand()
    {
        return new RelayCommand(
            _ =>
            {
                string? filepath = _fileDialogService.ShowSelectFileDialog("JPG images (*.jpeg, *.jpg)|*.jpeg;*.jpg");
                
                if (!string.IsNullOrWhiteSpace(filepath))
                {
                    ImageFilepath = filepath;
                }
            },
            _ => true);
    }   
    
    private RelayCommand CreateCreateOrEditCardCommand()
    {
        ActionType actionType = _cardForEditingViewModel == null ? ActionType.Create : ActionType.Edit;

        string actionGerund = actionType switch
        {
            ActionType.Create => "creating",
            ActionType.Edit => "editing",
            _ => throw new InvalidOperationException("Invalid enum value")
        };
                    
        string actionPastSimple = actionType switch
        {
            ActionType.Create => "created",
            ActionType.Edit => "edited",
            _ => throw new InvalidOperationException("Invalid enum value")
        };
        
        return new RelayCommand(
            async _ =>
            {
                try
                {
                    _applicationState.ShowPopup(new InfoViewModel($"We are {actionGerund} your card... Please wait patient."));

                    if (actionType == ActionType.Create)
                    {
                        await _cardsCRUDService.CreateAsync(new CardForCreationDto
                        {
                            Label = Label, 
                            ImageBytes = await File.ReadAllBytesAsync(ImageFilepath!)
                        });
                        
                        await _applicationState.RefreshCardsAsync();
                    }
                    
                    if (actionType == ActionType.Edit)
                    {
                        int id = _cardForEditingViewModel!.Id;
                        
                        bool isLocalImageFileExists = IsLocalImageFileExists();

                        var dto = new CardForUpdateDto
                        {
                            Label = Label, 
                            ImageBytes = isLocalImageFileExists ? await File.ReadAllBytesAsync(ImageFilepath!) : null
                        };

                        await _cardsCRUDService.UpdateAsync(id, dto);
                        
                        await _applicationState.RefreshCardsAsync();
                    }

                    _applicationState.ClosePopup();
                    
                    _applicationState.CurrentViewModel = new InfoViewModel($"Your card was successfully {actionPastSimple}!");
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "An error was occured during {ActionGerund} of an card", actionGerund);
                    
                    await _applicationState.ShowPopupAsync(new InfoViewModel($"An error was occured during {actionGerund} of your card."), 2);
                }
            },
            _ =>
            {
                return actionType switch
                {
                    ActionType.Create => !string.IsNullOrWhiteSpace(Label) && IsLocalImageFileExists(),
                    ActionType.Edit => !string.IsNullOrWhiteSpace(Label),
                    _ => false
                };
            });
    }

    private bool IsLocalImageFileExists()
    {
        return !string.IsNullOrWhiteSpace(ImageFilepath) && !Uri.IsWellFormedUriString(ImageFilepath, UriKind.Absolute) &&
            File.Exists(ImageFilepath);
    }
}