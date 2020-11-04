using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace FOOBAR.Shared
{
    public class LoggingHandler : DelegatingHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();
            sw.Start();
            if (request.Content != null && Log.IsEnabled(LogEventLevel.Debug))
            {

                Log.ForContext("Host", request.RequestUri.Host)
                    .ForContext("Headers", request.Headers)
                    .ForContext("Path", request.RequestUri.AbsolutePath)
                    .ForContext("Payload", await request.Content.ReadAsStringAsync())
                    .ForContext("Protocol", request.RequestUri.Scheme)
                    .ForContext("Method", request.Method)
                    .Information("API-call outgoing log Request");
            }
            else
            {
                Log.ForContext("Host", request.RequestUri.Host)
                    .ForContext("Headers", request.Headers)
                    .ForContext("Path", request.RequestUri.AbsolutePath)
                    .ForContext("Protocol", request.RequestUri.Scheme)
                    .ForContext("Method", request.Method)
                    .Information("API-call outgoing log Request");
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            sw.Stop();
            if (response.Content != null && (Log.IsEnabled(LogEventLevel.Debug) || response.StatusCode.ToString().StartsWith("4")))
            {
                Log.ForContext("Headers", response.Headers)
                    .ForContext("Payload",  await response.Content.ReadAsStringAsync())
                    .ForContext("Protocol", response.RequestMessage.RequestUri.Scheme)
                    .ForContext("Status", response.StatusCode)
                    .ForContext("Duration", sw.ElapsedMilliseconds)
                    .Information("API-call outgoing log Response");
            }
            else
            {
                Log.ForContext("Headers", response.Headers)
                    .ForContext("Protocol", response.RequestMessage.RequestUri.Scheme)
                    .ForContext("Status", response.StatusCode)
                    .ForContext("Duration", sw.ElapsedMilliseconds)
                    .Information("API-call outgoing log Response");
            }


            return response;
        }
    }
}
