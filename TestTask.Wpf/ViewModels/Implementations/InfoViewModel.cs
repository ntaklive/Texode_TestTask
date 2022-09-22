namespace TestTask.Wpf.ViewModels;

public class InfoViewModel: ViewModelBase, IInfoViewModel
{
    private string _text;

    public InfoViewModel()
    {
        _text = string.Empty;
    }
    
    public InfoViewModel(string text)
    {
        _text = text;
    }
    
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            
            OnPropertyChanged(nameof(Text));
        }
    }
}