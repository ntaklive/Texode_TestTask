using System.Text;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace TestTask.WebApi.DependencyInjection;

public static class LoggerBootstrapper
{
    public static void RegisterLogger(IServiceCollection services)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
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
            .WriteTo.Console(
                restrictedToMinimumLevel: LogEventLevel.Verbose,
                outputTemplate: "{Timestamp:HH:mm:ss:fff} [{Level:u3}] ({SourceContext}) {Message}{NewLine}{Exception}"
            )
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog();
        });
    }
}