using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestTask.Wpf.Services;
using TestTask.Wpf.Services.Abstractions;
using TestTask.Wpf.ViewModels;
using TestTask.Wpf.ViewModels.Factories.Implementations;
using TestTask.Wpf.ViewModels.Factories.Interfaces;

namespace TestTask.Wpf.DependencyInjection;

public static class FactoriesBootstrapper
{
    public static void RegisterFactories(IServiceCollection services)
    {
        services.AddSingleton<ICardViewModelFactory>(_ => new CardViewModelFactory());
        services.AddSingleton<ICardPreviewViewModelFactory>(provider => new CardPreviewViewModelFactory(
            provider.GetRequiredService<ApplicationState>(),
            provider.GetRequiredService<ICardCreateOrEditViewModelFactory>(),
            provider.GetRequiredService<ICardDeleteViewModelFactory>()
        ));
        services.AddSingleton<ICardDeleteViewModelFactory>(provider => new CardDeleteViewModelFactory(
            provider.GetRequiredService<ApplicationState>(),
            provider.GetRequiredService<ICardsCRUDService>()
        ));
        services.AddSingleton<ICardCreateOrEditViewModelFactory>(provider => new CardCreateOrEditViewModelFactory(
            provider.GetRequiredService<ApplicationState>(),
            provider.GetRequiredService<ISelectFileDialogService>(),
            provider.GetRequiredService<ICardsCRUDService>(),
            provider.GetRequiredService<ILogger<CardCreateOrEditViewModel>>()
        ));
    }
}