using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestTask.Shared.AutoMapper;
using TestTask.Shared.Services.Abstractions;
using TestTask.Wpf.AutoMapper;
using TestTask.Wpf.Helpers;
using TestTask.Wpf.Services;
using TestTask.Wpf.Services.Abstractions;
using JsonSerializer = TestTask.Shared.Services.JsonSerializer;

namespace TestTask.Wpf.DependencyInjection;

public static class ServicesBootstrapper
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(SharedMappingProfile), typeof(WpfMappingProfile));

        services.AddSingleton<IJsonSerializer>(provider => new JsonSerializer(
            provider.GetRequiredService<JsonSerializerOptions>()
        ));
        services.AddSingleton<JsonSerializerOptions>(_ => new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = {new JsonStringEnumConverter()},
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        services.AddScoped<IHttpClient>(provider => new HttpClient(
            provider.GetRequiredService<JsonSerializerOptions>()
        ));

        services.AddSingleton<ICardsCRUDService>(provider =>
            {
                string apiUrl = UriHelper.CombineUriToString(
                    provider.GetRequiredService<Configuration>().ServerUrl, "/api/cards");

                return new CardsCRUDService(
                    apiUrl,
                    provider.GetRequiredService<IHttpClient>(),
                    provider.GetRequiredService<IJsonSerializer>(),
                    provider.GetRequiredService<IMapper>(),
                    provider.GetRequiredService<ILogger<CardsCRUDService>>());
            }
        );

        services.AddSingleton<ApplicationState>(provider => new ApplicationState(
            provider.GetRequiredService<ICardsCRUDService>(),
            provider.GetRequiredService<ILogger<ApplicationState>>()
        ));
        
        services.AddSingleton<ISelectFileDialogService>(_ => new SelectFileDialogService());
    }
}