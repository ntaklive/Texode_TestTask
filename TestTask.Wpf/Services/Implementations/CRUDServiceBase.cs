using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TestTask.Shared.Entities.Abstractions.Interfaces;
using TestTask.Shared.Services.Abstractions;
using TestTask.Wpf.Services.Abstractions;

namespace TestTask.Wpf.Services;

public abstract class CRUDServiceBase<T, TDto, TDtoForCreation, TDtoForUpdate> : ICRUDService<T, TDtoForCreation, TDtoForUpdate>
    where T : class, IHasKey
    where TDto : class, IDto
    where TDtoForCreation : class, IDtoForCreation
    where TDtoForUpdate : class, IDtoForUpdate
{
    protected readonly string EndpointUrl;
    protected readonly IHttpClient HttpClient;
    protected readonly IJsonSerializer JsonSerializer;
    protected readonly IMapper Mapper;
    
    private readonly ILogger<CRUDServiceBase<T, TDto, TDtoForCreation, TDtoForUpdate>> _logger;

    protected CRUDServiceBase(string endpointUrl,
        IHttpClient httpClient,
        IJsonSerializer jsonSerializer,
        IMapper mapper,
        ILogger<CRUDServiceBase<T, TDto, TDtoForCreation, TDtoForUpdate>> logger)
    {
        JsonSerializer = jsonSerializer;
        Mapper = mapper;
        _logger = logger;
        EndpointUrl = endpointUrl;
        HttpClient = httpClient;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            string url = EndpointUrl;

            string json = await HttpClient.GetJsonAsync(url);
            _logger.LogInformation("An json from the {Url} url was successfully received", url);

            var itemsDtos = JsonSerializer.Deserialize<IEnumerable<TDto>>(json);

            var items = Mapper.Map<IEnumerable<T>>(itemsDtos);

            return items;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong inside GetAllAsync action");

            throw;
        }
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        try
        {
            string url = BuildEndpointUrlWithId(id);

            string json = await HttpClient.GetJsonAsync(url);
            _logger.LogInformation("An json from the {Url} url was successfully received", url);

            var itemsDtos = JsonSerializer.Deserialize<TDto>(json);

            var items = Mapper.Map<T>(itemsDtos);

            return items;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong inside GetByIdAsync action");

            throw;
        }
    }

    public virtual async Task<T> CreateAsync(TDtoForCreation value)
    {
        try
        {
            string url = EndpointUrl;

            var item = Mapper.Map<TDto>(value);

            string json = JsonSerializer.Serialize(item);

            HttpResponseMessage response = await HttpClient.PostJsonAsync(url, json);
            _logger.LogInformation("An create request to the {Url} url was successfully handled", url);

            var createdItem = Mapper.Map<T>(JsonSerializer.Deserialize<TDto>(await response.Content.ReadAsStringAsync()));

            return createdItem;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong inside CreateAsync action");

            throw;
        }
    }

    public virtual async Task UpdateAsync(int id, TDtoForUpdate value)
    {
        try
        {
            string url = BuildEndpointUrlWithId(id);

            var item = Mapper.Map<TDto>(value);

            string json = JsonSerializer.Serialize(item);

            await HttpClient.PutJsonAsync(url, json);
            _logger.LogInformation("An put request to the {Url} url was successfully handled", url);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong inside UpdateAsync action");

            throw;
        }
    }

    public virtual async Task DeleteAsync(int id)
    {
        try
        {
            string url = BuildEndpointUrlWithId(id);

            await HttpClient.DeleteAsync(url);
            _logger.LogInformation("An delete request to the {Url} url was successfully handled", url);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong inside UpdateAsync action");

            throw;
        }
    }

    protected string BuildEndpointUrlWithId(int id)
    {
        return $"{EndpointUrl}/{id}";
    }
}