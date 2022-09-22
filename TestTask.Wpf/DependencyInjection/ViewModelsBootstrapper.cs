using Microsoft.Extensions.DependencyInjection;
using TestTask.Wpf.Services;
using TestTask.Wpf.ViewModels;
using TestTask.Wpf.ViewModels.Factories.Interfaces;

namespace TestTask.Wpf.DependencyInjection;

public static class ViewModelsBootstrapper
{
    public static void RegisterViewModels(IServiceCollection services)
    {
        services.AddTransient<IMainWindowViewModel>(provider => new MainWindowViewModel(
            provider.GetRequiredService<ApplicationState>(),
            provider.GetRequiredService<ICardViewModelFactory>(),
            provider.GetRequiredService<ICardPreviewViewModelFactory>(),
            provider.GetRequiredService<ICardCreateOrEditViewModelFactory>()
            ));
    }
}