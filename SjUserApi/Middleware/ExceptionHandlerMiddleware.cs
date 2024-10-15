using System.Net;
using System.Text.Json;
using Serilog;
using SjUserApi.Exceptions;

namespace SjUserApi.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception in request {Path}", context.Request.Path);
            
            var (code, message) = GetExceptionMessageAndCode(ex);
            
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int) code;
            await response.WriteAsync(message);
        }
    }

    private static (HttpStatusCode code, string message) GetExceptionMessageAndCode(Exception exception)
    {
        HttpStatusCode code;

        switch (exception)
        {
            case UserNotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            default:
                code = HttpStatusCode.InternalServerError;
                break;
        }

        return (code, JsonSerializer.Serialize(exception.Message));
    }
}