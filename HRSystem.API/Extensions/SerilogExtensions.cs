using Serilog;
using Serilog.Formatting.Json;

namespace HRSystem.API.Extensions
{
    public static class SerilogExtensions
    {
        public static void AddSerilogLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Application", "HRSystem.API")
                .WriteTo.Console(new JsonFormatter(renderMessage: true))
                .WriteTo.File(
                    new JsonFormatter(renderMessage: true),
                    "logs/log-.json",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
