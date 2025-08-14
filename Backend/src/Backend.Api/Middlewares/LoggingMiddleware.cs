using Microsoft.AspNetCore.Http;
using Serilog;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        var userName = context.User?.Identity?.IsAuthenticated == true
            ? context.User.Identity.Name
            : "Anonymous";

        var endpoint = $"{context.Request.Method} {context.Request.Path}";

        string bodyContent = string.Empty;
        if (context.Request.ContentLength > 0 &&
            (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put))
        {
            using (var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true))
            {
                bodyContent = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }
        }

        Log.Information("{User} kullanıcısı {Endpoint} endpointine istek yaptı. Body: {Body}",
            userName, endpoint, bodyContent);

        await _next(context);
    }
}
