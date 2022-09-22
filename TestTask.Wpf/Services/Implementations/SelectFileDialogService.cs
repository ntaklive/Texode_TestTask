using TestTask.Wpf.Services.Abstractions;

namespace TestTask.Wpf.Services;

public class SelectFileDialogService : ISelectFileDialogService
{
    public string? ShowSelectFileDialog(string filter)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = filter
        };

        bool? result = dialog.ShowDialog();
        
        return result == true ? dialog.FileName : null;
    }
}