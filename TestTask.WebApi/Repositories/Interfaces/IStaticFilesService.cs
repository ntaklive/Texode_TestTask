using System.Drawing.Imaging;

namespace TestTask.WebApi.Repositories;

public interface IStaticFilesService
{
    public string[] SupportedImageExtensions { get; }
    
    public string GetFilesDirectoryPath();
    public string GetFilesRequestPath();

    public (string LocalFilepath, string RequestPath) CreateImageFromBytes(byte[] bytes, string filename, ImageFormat format);
}