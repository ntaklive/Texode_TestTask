using System.ComponentModel;
using System.Runtime.CompilerServices;
using TestTask.Shared;

namespace TestTask.Wpf.ViewModels;

public abstract class ViewModelBase : IViewModel
{
    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}