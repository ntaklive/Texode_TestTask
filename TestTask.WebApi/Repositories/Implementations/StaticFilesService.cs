using System.Drawing.Imaging;
using System.Drawing;
using System.Reflection;

namespace TestTask.WebApi.Repositories;

public sealed class StaticFilesService : IStaticFilesService
{
    public StaticFilesService()
    {
        Directory.CreateDirectory(GetFilesDirectoryPath());

        SupportedImageExtensions = new[] {"jpg", "jpeg"};
    }

    public string[] SupportedImageExtensions { get; }

    public string GetFilesDirectoryPath()
    {
        return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "wwwroot\\files");
    }

    public string GetFilesRequestPath()
    {
        return "/dl";
    }

    public (string LocalFilepath, string RequestPath) CreateImageFromBytes(byte[] bytes, string filename,
        ImageFormat format)
    {
        using Image image = Image.FromStream(new MemoryStream(bytes));

        string filepath = Path.Combine(GetFilesDirectoryPath(), filename);
        string requestPath = $"{GetFilesRequestPath()}/{filename}";

        image.Save(filepath, format);

        return new ValueTuple<string, string>(filepath, requestPath);
    }
}