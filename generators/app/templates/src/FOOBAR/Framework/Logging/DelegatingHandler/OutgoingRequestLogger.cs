using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using FOOBAR.Shared.Options.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Serilog;

namespace FOOBAR.Framework.Logging.DelegatingHandler
{
	public class OutgoingRequestLogger<T> : System.Net.Http.DelegatingHandler
	{
		private readonly LogSettings _logSettings;

		public OutgoingRequestLogger(IOptions<LogSettings> logSettings)
		{
			_logSettings = logSettings.Value ??
			               throw new ArgumentNullException(
				               $"{nameof(OutgoingRequestLogger<T>)}.Ctr parameter {nameof(logSettings)} cannot be null.");
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			if (!_logSettings.RequestLogging.OutgoingEnabled)
			{
				return await base.SendAsync(request, cancellationToken);
			}

			HttpResponseMessage response = null;
			var sw = new Stopwatch();

			try
			{
				sw.Start();

				response = await base.SendAsync(request, cancellationToken);
			}
			finally
			{
				sw.Stop();
				await LogRequest(request, response, sw);
			}

			return response;
		}

		private async Task LogRequest(HttpRequestMessage request, HttpResponseMessage response, Stopwatch sw)
		{
			// create Ilogger and enricht with extra properties
			Log
				.ForContext("Type", new[] { "application" })
				.ForContext("Request", await GetRequestLog(request))
				.ForContext("Response", await GetResponseLog(response, sw.ElapsedMilliseconds))
				.Information($"{nameof(T)} outgoing API call response");
		}

		private static bool ShouldLogHeader(string name, IEnumerable<string> allowedHeaders)
		{
			return allowedHeaders.ToList().Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
		}

		private static Dictionary<string, string> GetHeaders(HttpHeaders headers, IEnumerable<string> allowedHeaders)
		{
			var result = new Dictionary<string, string>();

			var allowedHeaderList = allowedHeaders?.ToList();
			if (allowedHeaderList == null || !allowedHeaderList.Any())
			{
				return result;
			}

			headers
				.Where(x => ShouldLogHeader(x.Key, allowedHeaderList))
				.ToList()
				.ForEach(x => result.Add(x.Key, string.Join(",", x.Value)));

			return result;
		}

		private static string GetContentType(HttpHeaders headers)
		{
			headers.TryGetValues(HeaderNames.ContentType, out var contentTypeHeader);

			return contentTypeHeader?.FirstOrDefault();
		}

		private bool ShouldLogRequestPayload()
		{
			return _logSettings.RequestLogging.LogPayload;
		}

		private static async Task<string> GetRequestBody(HttpRequestMessage request)
		{
			if (GetContentType(request.Headers)?.Contains("multipart/form-data") == true)
			{
				return "Logging of multipart content body disabled";
			}

			if (request.Content == null)
			{
				return null;
			}

			return await request.Content.ReadAsStringAsync();
		}

		private async Task<Dictionary<string, object>> GetRequestLog(HttpRequestMessage request)
		{
			var requestLog = new Dictionary<string, object>
			{
				{ "method", request.Method },
				{ "host", request.RequestUri?.Host },
				{ "path", request.RequestUri?.PathAndQuery },
				{ "protocol", request.RequestUri?.Scheme },
				{ "headers", GetHeaders(request.Headers, _logSettings.RequestLogging.AllowedOutgoingRequestHeaders) }
			};

			if (ShouldLogRequestPayload())
			{
				requestLog.Add("payload", await GetRequestBody(request));
			}

			return requestLog;
		}

		private bool ShouldLogResponsePayload(HttpResponseMessage response)
		{
			return _logSettings.RequestLogging.LogPayload
			       || (_logSettings.RequestLogging.LogPayloadOnError && (int)response.StatusCode >= 400 &&
			           (int)response.StatusCode < 500);
		}

		private static async Task<string> GetResponseBody(HttpResponseMessage response)
		{
			if (GetContentType(response.Headers)?.Contains("multipart/form-data") == true)
			{
				return "Logging of multipart content body disabled";
			}

			return await response.Content.ReadAsStringAsync();
		}

		private async Task<Dictionary<string, object>> GetResponseLog(HttpResponseMessage response, long durationInMs)
		{
			var responseLog = new Dictionary<string, Object> { { "duration", durationInMs } };

			if (response != null)
			{
				responseLog.Add("headers",
					GetHeaders(response.Headers, _logSettings.RequestLogging.AllowedOutgoingResponseHeaders));
				responseLog.Add("status", (int)response.StatusCode);

				if (ShouldLogResponsePayload(response))
				{
					responseLog.Add("payload", await GetResponseBody(response));
				}
			}

			return responseLog;
		}
	}
}
