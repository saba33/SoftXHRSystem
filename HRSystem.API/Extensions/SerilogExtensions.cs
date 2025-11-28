using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;

namespace HRSystem.API.Extensions
{
    public static class SerilogExtensions
    {
        public static void AddSerilogLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)

                .WriteTo.File(
                    path: "logs/log-.txt",
                    rollingInterval: RollingInterval.Day
                )

                .WriteTo.Async(a => a.File(
                    path: "logs/error-.json",
                    rollingInterval: RollingInterval.Day,
                    formatter: new JsonFormatter() 
                ))

                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
