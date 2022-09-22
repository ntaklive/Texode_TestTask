using TestTask.Shared.Entities;

namespace TestTask.Wpf.ViewModels;

public class CardViewModel : ViewModelBase, ICardViewModel
{
    private string _label;
    private string? _imageUrl;

    public CardViewModel()
    {
        _label = "Unknown";
        _imageUrl = string.Empty;

        Id = 0;
    }

    public CardViewModel(Card card)
    {
        _label = card.Label;
        _imageUrl = card.ImageUrl;
        
        Id = card.Id;
    }

    public int Id { get; }

    public string Label
    {
        get => _label;
        set
        {
            _label = value;
            OnPropertyChanged(nameof(Label));
        }
    }

    public string? ImageUrl
    {
        get => _imageUrl;
        set
        {
            _imageUrl = value;
            OnPropertyChanged(nameof(ImageUrl));
        }
    }
}