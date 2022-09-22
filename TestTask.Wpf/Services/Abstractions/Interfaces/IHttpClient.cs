using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TestTask.Wpf.Services.Abstractions.Models;

namespace TestTask.Wpf.Services.Abstractions;

public interface IHttpClient : IDisposable
{
    public Task<string> GetJsonAsync(string url, IProgress<HttpDownloadProgress>? progress = null,
        CancellationToken cancellationToken = default);

    public Task<byte[]> GetBytesAsync(string url, IProgress<HttpDownloadProgress>? progress = null,
        CancellationToken cancellationToken = default);

    public Task<HttpResponseMessage> PostJsonAsync(string url, string json, CancellationToken cancellationToken = default);

    public Task<HttpResponseMessage> PostAsync(string url, HttpContent content, CancellationToken cancellationToken = default);

    public Task<HttpResponseMessage> PutJsonAsync(string url, string json, CancellationToken cancellationToken = default);

    public Task<HttpResponseMessage> PutAsync(string url, HttpContent content, CancellationToken cancellationToken = default);

    public Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancellationToken = default);

}