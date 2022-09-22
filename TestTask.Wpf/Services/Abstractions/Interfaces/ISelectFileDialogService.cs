namespace TestTask.Wpf.Services.Abstractions;

public interface ISelectFileDialogService
{
    /// <summary>
    /// Show select file dialog
    /// </summary>
    /// <param name="filter">Example:<br/>"JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg"</param>
    /// <returns>Absolute filepath to the selected file or null if nothing selected</returns>
    public string? ShowSelectFileDialog(string filter);
}