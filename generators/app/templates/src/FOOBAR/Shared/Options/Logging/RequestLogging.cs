using System.Collections.Generic;

namespace FOOBAR.Shared.Options.Logging
{
	public class RequestLogging
	{
		public readonly IEnumerable<string> DefaultAllowedRequestHeaders = new List<string>
		{
			// Standard request headers
			"Accept",
			"Accept-Charset",
			"Accept-Datetime",
			"Accept-Encoding",
			"Accept-Language",
			"Access-Control-Request-Method",
			"Access-Control-Request-Headers",
			// "Authorization",
			"Cache-Control",
			"Connection",
			"Content-Encoding",
			"Content-Length",
			"Content-Type",
			// "Cookie",
			"Date",
			"Expect",
			"Forwarded",
			"From",
			"Host",
			"HTTP2-Settings",
			"If-Match",
			"If-Modified-Since",
			"If-None-Match",
			"If-Range",
			"If-Unmodified-Since",
			"Max-Forwards",
			"Origin",
			"Pragma",
			"Prefer",
			// "Proxy-Authorization",
			"Range",
			"Referer",
			"TE",
			"Trailer",
			"Transfer-Encoding",
			"User-Agent",
			"Upgrade",
			"Via",
			"Warning",

			// Non-standard request headers
			"X-Forwarded-For",
			"X-Forwarded-Host",
			"X-Forwarded-Proto",

			// Digipolis-specific request headers
			"Dgp-Correlation"
		};

		public readonly IEnumerable<string> DefaultAllowedResponseHeaders = new List<string>
		{
			// Standard response headers
			"Accept-CH",
			"Access-Control-Allow-Origin",
			"Access-Control-Allow-Credentials",
			"Access-Control-Expose-Headers",
			"Access-Control-Max-Age",
			"Access-Control-Allow-Methods",
			"Access-Control-Allow-Headers",
			"Accept-Patch",
			"Accept-Ranges",
			"Age",
			"Allow",
			"Alt-Svc",
			"Cache-Control",
			"Connection",
			"Content-Disposition",
			"Content-Encoding",
			"Content-Language",
			"Content-Length",
			"Content-Location",
			"Content-Range",
			"Content-Type",
			"Date",
			"Delta-Base",
			"ETag",
			"Expires",
			"IM",
			"Last-Modified",
			"Link",
			"Location",
			"P3P",
			"Pragma",
			"Preference-Applied",
			"Proxy-Authenticate",
			"Public-Key-Pins",
			"Retry-After",
			"Server",
			// "Set-Cookie",
			"Strict-Transport-Security",
			"Trailer",
			"Transfer-Encoding",
			"Tk",
			"Upgrade",
			"Vary",
			"Via",
			"Warning",
			"WWW-Authenticate"
		};

		public RequestLogging()
		{
			AllowedIncomingRequestHeaders = DefaultAllowedRequestHeaders;
			AllowedIncomingResponseHeaders = DefaultAllowedResponseHeaders;
			AllowedOutgoingRequestHeaders = DefaultAllowedRequestHeaders;
			AllowedOutgoingResponseHeaders = DefaultAllowedResponseHeaders;
		}

		// GENERAL
		public bool LogPayload { get; set; }
		public bool LogPayloadOnError { get; set; }

		// REQUEST LOGGING - INCOMING (middleware)
		public bool IncomingEnabled { get; set; }
		public IEnumerable<string> AllowedIncomingRequestHeaders { get; set; }
		public IEnumerable<string> AllowedIncomingResponseHeaders { get; set; }

		// REQUEST LOGGING - OUTGOING (delegating handler)
		public bool OutgoingEnabled { get; set; }
		public IEnumerable<string> AllowedOutgoingRequestHeaders { get; set; }
		public IEnumerable<string> AllowedOutgoingResponseHeaders { get; set; }
	}
}
