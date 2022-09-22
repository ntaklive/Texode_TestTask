namespace TestTask.WebApi.DependencyInjection;

public static class Bootstrapper
{
    public static void Register(IServiceCollection services)
    {
        LoggerBootstrapper.RegisterLogger(services);
        ServicesBootstrapper.RegisterServices(services);
    }
}