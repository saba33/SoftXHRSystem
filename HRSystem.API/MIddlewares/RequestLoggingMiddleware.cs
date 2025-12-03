using Serilog;
using System.Diagnostics;

namespace HRSystem.API.MIddlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            var request = context.Request;
            var method = request.Method;
            var path = request.Path + request.QueryString;
            var ip = context.Connection.RemoteIpAddress?.ToString();

            await _next(context);

            sw.Stop();

            Log.ForContext("Method", method)
               .ForContext("Path", path)
               .ForContext("Elapsed", sw.ElapsedMilliseconds)
               .ForContext("IP", ip)
               .ForContext("StatusCode", context.Response.StatusCode)
               .Information("Request completed");
        }
    }
}
