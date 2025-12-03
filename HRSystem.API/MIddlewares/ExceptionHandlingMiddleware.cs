using Serilog;
using System.Net;

namespace HRSystem.API.MIddlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid().ToString();

                Log.ForContext("ErrorId", errorId)
                   .ForContext("Path", context.Request.Path)
                   .ForContext("Query", context.Request.QueryString.Value ?? "")
                   .ForContext("Method", context.Request.Method)
                   .ForContext("IP", context.Connection.RemoteIpAddress?.ToString())
                   .Error(ex, "Unhandled exception");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = new
                {
                    error = new
                    {
                        id = errorId,
                        message = "Internal server error occurred.",
                        traceId = context.TraceIdentifier,
                        timestamp = DateTime.UtcNow.ToString("o"),
                        path = context.Request.Path
                    }
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
