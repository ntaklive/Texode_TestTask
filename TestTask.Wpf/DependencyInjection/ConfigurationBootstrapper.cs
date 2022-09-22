using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestTask.Wpf.DependencyInjection;

public static class ConfigurationBootstrapper
{
    public static void RegisterConfiguration(IServiceCollection services)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("settings.json")
            .Build();

        services.AddSingleton<Configuration>(configuration.Get<Configuration>());
    }
}