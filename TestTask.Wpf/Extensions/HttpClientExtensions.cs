using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TestTask.Wpf.Services.Abstractions.Models;

namespace TestTask.Wpf.Extensions;

public static class HttpClientExtensions
{
    private const int BufferSize = 8192;

    public static async Task<byte[]> GetByteArrayAsync(this HttpClient client, Uri requestUri, IProgress<HttpDownloadProgress>? progress, CancellationToken cancellationToken)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        using HttpResponseMessage responseMessage = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        
        responseMessage.EnsureSuccessStatusCode();

        HttpContent? content = responseMessage.Content;
        if (content == null!)
        {
            return Array.Empty<byte>();
        }

        HttpContentHeaders headers = content.Headers;
        long? contentLength = headers.ContentLength;
        
        await using Stream responseStream = await content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        
        var buffer = new byte[BufferSize];
        int bytesRead;
        var bytes = new List<byte>();

        var downloadProgress = new HttpDownloadProgress();
        if (contentLength.HasValue)
        {
            downloadProgress.TotalBytesToReceive = (ulong)contentLength.Value;
        }
        progress?.Report(downloadProgress);

        while ((bytesRead = await responseStream.ReadAsync(buffer.AsMemory(0, BufferSize), cancellationToken).ConfigureAwait(false)) > 0)
        {
            bytes.AddRange(buffer.Take(bytesRead));

            downloadProgress.BytesReceived += (ulong)bytesRead;
            progress?.Report(downloadProgress);
        }

        return bytes.ToArray();
    }
}

