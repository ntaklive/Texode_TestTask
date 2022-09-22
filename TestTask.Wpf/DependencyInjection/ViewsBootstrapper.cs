using Microsoft.Extensions.DependencyInjection;
using TestTask.Wpf.Views;

namespace TestTask.Wpf.DependencyInjection;

public static class ViewsBootstrapper
{
    public static void RegisterViews(IServiceCollection services)
    {
        services.AddTransient<MainWindow>();
    }
}