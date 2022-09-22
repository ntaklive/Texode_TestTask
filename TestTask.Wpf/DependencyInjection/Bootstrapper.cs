using Microsoft.Extensions.DependencyInjection;

namespace TestTask.Wpf.DependencyInjection;

public static class Bootstrapper
{
    public static void Register(IServiceCollection services)
    {
        LoggerBootstrapper.RegisterLogger(services);
        ServicesBootstrapper.RegisterServices(services);
        ViewModelsBootstrapper.RegisterViewModels(services);
        FactoriesBootstrapper.RegisterFactories(services);
        ViewsBootstrapper.RegisterViews(services);
        ConfigurationBootstrapper.RegisterConfiguration(services);
    }
}