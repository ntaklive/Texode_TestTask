using System;
using System.Windows.Input;

namespace TestTask.Wpf.Helpers;

public class RelayCommand : ICommand
{
    private readonly Action<object?> _action;
    private readonly Func<object?, bool>? _canExecute;

    public RelayCommand(Action<object?> action, Func<object?, bool> canExecute)
    {
        _action = action;
        _canExecute = canExecute;
    }


    public bool CanExecute(object? parameter)
    {
        return _canExecute != null && _canExecute(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public void Execute(object? parameter)
    {
        _action(parameter!);
    }
}