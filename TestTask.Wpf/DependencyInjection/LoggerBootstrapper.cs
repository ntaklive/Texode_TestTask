using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace TestTask.Wpf.DependencyInjection;

public static class LoggerBootstrapper
{
    public static void RegisterLogger(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.WithExceptionDetails()
            .Enrich.FromLogContext()
            .WriteTo.File(
                "logs/log.txt",
                restrictedToMinimumLevel: LogEventLevel.Verbose,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day
            )
            .WriteTo.File(
                "logs/errors_log.txt",
                restrictedToMinimumLevel: LogEventLevel.Warning,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day
            )
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog();
        });
    }
}