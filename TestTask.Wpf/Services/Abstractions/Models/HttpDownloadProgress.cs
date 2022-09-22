namespace TestTask.Wpf.Services.Abstractions.Models;

public struct HttpDownloadProgress
{
    public ulong BytesReceived { get; set; }

    public ulong? TotalBytesToReceive { get; set; }
}