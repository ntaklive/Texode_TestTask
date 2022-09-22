using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TestTask.Shared.Dto;
using TestTask.Shared.Entities;
using TestTask.Shared.Services.Abstractions;
using TestTask.Wpf.Dto;
using TestTask.Wpf.Services.Abstractions;

namespace TestTask.Wpf.Services;

public sealed class CardsCRUDService : CRUDServiceBase<Card, CardDto, CardForCreationDto, CardForUpdateDto>, ICardsCRUDService
{
    private readonly ILogger<CardsCRUDService> _logger;

    public CardsCRUDService(
        string endpointUrl,
        IHttpClient httpClient,
        IJsonSerializer jsonSerializer,
        IMapper mapper,
        ILogger<CardsCRUDService> logger) 
        : base(endpointUrl, httpClient, jsonSerializer, mapper, logger)
    {
        _logger = logger;
    }

    public override async Task<Card> CreateAsync(CardForCreationDto value)
    {
        try
        {
            string url = EndpointUrl;
            
            using var formData = new MultipartFormDataContent();
            
            formData.Add(new StreamContent(new MemoryStream(value.ImageBytes)), "File", $"{Guid.NewGuid().ToString().Replace("-", "")}.jpg");
            formData.Add(new StringContent(value.Label, Encoding.UTF8, "text/plain"), "Label");

            HttpResponseMessage response = await HttpClient.PostAsync(url, formData);
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Cannot create an entry from this value");
            }

            _logger.LogInformation("An create request to the {Url} url was successfully handled", url);
            
            var createdItem = Mapper.Map<Card>(JsonSerializer.Deserialize<CardDto>(await response.Content.ReadAsStringAsync()));

            return createdItem;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong inside CreateAsync action");

            throw;
        }
    }

    public override async Task UpdateAsync(int id, CardForUpdateDto value)
    {
        try
        {
            string url = BuildEndpointUrlWithId(id);
            
            using var formData = new MultipartFormDataContent();
            if (value.ImageBytes != null)
            {
                formData.Add(new StreamContent(new MemoryStream(value.ImageBytes)), "File", $"{Guid.NewGuid().ToString().Replace("-", "")}.jpg");
            }
            
            formData.Add(new StringContent(value.Label, Encoding.UTF8, "text/plain"), "Label");

            HttpResponseMessage response = await HttpClient.PutAsync(url, formData);
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Cannot update an entry from this value");
            }

            _logger.LogInformation("An update request to the {Url} url was successfully handled", url);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong inside UpdateAsync action");

            throw;
        }    }
}