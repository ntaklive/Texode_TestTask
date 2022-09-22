using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestTask.Wpf.DependencyInjection;
using TestTask.Wpf.Services;
using TestTask.Wpf.Views;

namespace TestTask.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices(Bootstrapper.Register)
                .Build();
            
            AppState = AppHost.Services.GetRequiredService<ApplicationState>();
        }

        public static IHost AppHost { get; private set; } = null!;
        public static ApplicationState AppState { get; private set; } = null!;
        private static ILogger<App> Logger { get; set; } = null!;

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();

            Logger = AppHost.Services.GetRequiredService<ILogger<App>>();
            
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();

            mainWindow.Show();

            await AppState.RefreshCardsAsync();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            
            base.OnExit(e);
        }
    }
}