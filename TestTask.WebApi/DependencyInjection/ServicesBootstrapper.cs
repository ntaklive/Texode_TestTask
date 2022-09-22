using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using TestTask.Shared.AutoMapper;
using TestTask.Shared.Services.Abstractions;
using TestTask.WebApi.Repositories;
using JsonSerializer = TestTask.Shared.Services.JsonSerializer;

namespace TestTask.WebApi.DependencyInjection;

public static class ServicesBootstrapper
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(SharedMappingProfile));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<RepositoryContext>(provider => new RepositoryContext(
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "wwwroot", "data"),
            provider.GetRequiredService<IJsonSerializer>(),
            provider.GetRequiredService<ILogger<RepositoryContext>>()));

        services.AddTransient<ICardRepository, CardRepository>();

        services.AddSingleton<IJsonSerializer>(_ => new JsonSerializer(
            new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = {new JsonStringEnumConverter()},
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        ));

        services.AddSingleton<IStaticFilesService>(_ => new StaticFilesService());
    }
}