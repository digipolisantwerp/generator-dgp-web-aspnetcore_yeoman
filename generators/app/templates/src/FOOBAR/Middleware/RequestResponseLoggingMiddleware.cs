using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace FOOBAR.Middleware
{
    public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var sw = new Stopwatch();
        sw.Start();
        //First, get the incoming request
        await LogRequest(context.Request);

        //Copy a pointer to the original response body stream
        var originalBody = context.Response.Body;
        try
        {
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);
                responseBody.Position = 0;
                await LogResponse(context.Response, sw);
                responseBody.Position = 0;
                await responseBody.CopyToAsync(originalBody);
            }
        }
        finally
        {
            //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            context.Response.Body = originalBody;
        }

    }

    private async Task LogRequest(HttpRequest request)
    {
        // if debug is enabled we need to log the payload
        if (Log.IsEnabled(LogEventLevel.Debug))
        {
            var body = request.Body;

            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body = body;

            Log.ForContext("Host", request.Host)
                .ForContext("Headers", request.Headers)
                .ForContext("Path", request.Path)
                .ForContext("Payload", bodyAsText)
                .ForContext("Protocol", request.Protocol)
                .ForContext("Method", request.Method)
                .Information("API-call incoming log Request");
        }
        else
        {
            Log.ForContext("Host", request.Host)
                .ForContext("Headers", request.Headers)
                .ForContext("Path", request.Path)
                .ForContext("Protocol", request.Protocol)
                .ForContext("Method", request.Method)
                .Information("API-call incoming log Request");
        }

    }

    private async Task LogResponse(HttpResponse response, Stopwatch sw)
    {
        // if debug is enabled or if status code is 4xx we need to log the payload
        if (Log.IsEnabled(LogEventLevel.Debug) || response.StatusCode.ToString().StartsWith("4"))
        {

            response.Body.Seek(0, SeekOrigin.Begin);
            string body = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            sw.Stop();

            Log.ForContext("Headers", response.Headers)
                .ForContext("Payload", body)
                .ForContext("Protocol", response.HttpContext.Request.Protocol)
                .ForContext("Status", response.StatusCode)
                .ForContext("Duration", sw.ElapsedMilliseconds)
                .Information("API-call incoming log Response");
        }
        else
        {
            sw.Stop();

            Log.ForContext("Headers", response.Headers)
                .ForContext("Protocol", response.HttpContext.Request.Protocol)
                .ForContext("Status", response.StatusCode)
                .ForContext("Duration", sw.ElapsedMilliseconds)
                .Information("API-call incoming log Response");
        }

    }
}
}
