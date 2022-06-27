using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Digipolis.Auth.Providers;
using FOOBAR.Shared.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FOOBAR.Mvc.Controllers
{
	public class PassThroughController : Controller
	{
		private readonly AppSettings _appSettings;
		private readonly ITokenProvider _tokenService;

		public PassThroughController(IOptions<AppSettings> appSettings, ITokenProvider tokenService)
		{
			_tokenService = tokenService;
			_appSettings = appSettings?.Value ??
			               throw new ArgumentNullException(
				               $"{nameof(PassThroughController)}.ctor() : {nameof(appSettings)} cannot be null");
		}

		public async Task Handle()
		{
			var apiUrl = _appSettings.ApiUrl;

			var accessToken = await _tokenService.GetOAuthAccessToken();
			var originalRequest = HttpContext.Request;

#if DEBUG
			accessToken = _appSettings.TestingJwt;
#endif

			var prefix =
				@"/" + AppSettings.PassThroughPrefix; // value is const, must be gotten from class instead of instance
			var path = HttpContext.Request.Path.ToString().Remove(0, prefix.Length);

			var uri = apiUrl + path;

			//TODO: replace with https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-6.0
			//WebRequest.Create is obsolete for new dev purposes
			var request = (HttpWebRequest)WebRequest.Create(uri + HttpContext.Request.QueryString);

			request.Method = HttpContext.Request.Method;
			request.ContentType = HttpContext.Request.ContentType;

			request.Headers["ApiKey"] = _appSettings.ApiKey;
			request.Headers["Authorization"] = "Bearer " + accessToken;

			//1000 * 60 * 10
			request.ContinueTimeout = 600000;

			if (originalRequest.ContentLength.HasValue)
			{
				await using var requestBodyStream = originalRequest.Body;
				byte[] requestBodyBytes;
				using (var br = new BinaryReader(requestBodyStream))
				{
					requestBodyBytes = br.ReadBytes((int)originalRequest.ContentLength);
				}

				await using var requestStream = await request.GetRequestStreamAsync();
				await requestStream.WriteAsync(requestBodyBytes, 0, requestBodyBytes.Length);
				await requestStream.FlushAsync();
			}

			try
			{
				using var response = await request.GetResponseAsync();
				HttpContext.Response.ContentType = response.ContentType;
				HttpContext.Response.StatusCode = (int)((HttpWebResponse)response).StatusCode;

				if (response.Headers["x-filename"] != null)
					HttpContext.Response.Headers["x-filename"] = response.Headers["x-filename"];

				if (response.Headers["content-disposition"] != null)
					HttpContext.Response.Headers["content-disposition"] = response.Headers["content-disposition"];

				await using var responseStream = response.GetResponseStream();
				await responseStream?.CopyToAsync(HttpContext.Response.Body)!;
				await HttpContext.Response.Body.FlushAsync();
			}
			catch (WebException ex)
			{
				if (ex.Status == WebExceptionStatus.ConnectFailure || ex.Status == WebExceptionStatus.UnknownError)
				{
					HttpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
					return;
				}

				HttpContext.Response.ContentType = ((HttpWebResponse)ex.Response)?.ContentType ?? string.Empty;
				HttpContext.Response.StatusCode = (int)((HttpWebResponse)ex.Response)?.StatusCode;

				await using var responseStream = ex.Response?.GetResponseStream();
				await responseStream?.CopyToAsync(HttpContext.Response.Body)!;
				await HttpContext.Response.Body.FlushAsync();
			}
		}
	}
}
