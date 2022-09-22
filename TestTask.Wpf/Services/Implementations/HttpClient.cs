using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TestTask.Wpf.Extensions;
using TestTask.Wpf.Services.Abstractions;
using TestTask.Wpf.Services.Abstractions.Models;

namespace TestTask.Wpf.Services;

public sealed class HttpClient : IHttpClient
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly System.Net.Http.HttpClient _client;

    public HttpClient(JsonSerializerOptions jsonSerializerOptions, HttpMessageHandler? httpMessageHandler = null)
    {
        _jsonSerializerOptions = jsonSerializerOptions;
        _client = httpMessageHandler == null
            ? new System.Net.Http.HttpClient()
            : new System.Net.Http.HttpClient(httpMessageHandler);
        _client.DefaultRequestHeaders.Add("User-Agent",
            @"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
    }

    public async Task<string> GetJsonAsync(string url, IProgress<HttpDownloadProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        return ConvertBytesToString(await GetBytesAsync(url, progress, cancellationToken));
    }

    public async Task<byte[]> GetBytesAsync(string url, IProgress<HttpDownloadProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        return await _client.GetByteArrayAsync(new Uri(url), progress, cancellationToken);
    }

    public async Task<HttpResponseMessage> PostJsonAsync(string url, string json, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsJsonAsync(new Uri(url), json, _jsonSerializerOptions, cancellationToken);
    }
    
    public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content, CancellationToken cancellationToken = default)
    {
        return await _client.PostAsync(new Uri(url), content, cancellationToken);
    }

    public async Task<HttpResponseMessage> PutJsonAsync(string url, string json, CancellationToken cancellationToken = default)
    {
        return await _client.PutAsJsonAsync(new Uri(url), json, _jsonSerializerOptions, cancellationToken);
    }

    public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content, CancellationToken cancellationToken = default)
    {
        return await _client.PutAsync(new Uri(url), content, cancellationToken);
    }
    
    public async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancellationToken = default)
    {
        return await _client.DeleteAsync(new Uri(url), cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    private static string ConvertBytesToString(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }
}