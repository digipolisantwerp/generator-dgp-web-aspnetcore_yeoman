using FOOBAR.Shared.Options;
using Digipolis.Authentication.OAuth.Options;
using Digipolis.Authentication.OAuth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FOOBAR.Controllers
{
    public class PassThroughController : Controller
    {
        private readonly OAuthOptions _options;
        private readonly IOAuthService _service;
        private readonly AppSettings _appSettings;

        public PassThroughController(IOptions<OAuthOptions> options, IOAuthService service, IOptions<AppSettings> appSettings)
        {
            _options = options?.Value ?? throw new ArgumentNullException($"{nameof(PassThroughController)}.ctor() : {nameof(options)} cannot be null");
            _service = service ?? throw new ArgumentNullException($"{nameof(PassThroughController)}.ctor() : {nameof(service)} cannot be null");
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException($"{nameof(PassThroughController)}.ctor() : {nameof(appSettings)} cannot be null");
        }

        public async Task Handle()
        {
            var apiUrl = _appSettings.ApiUrl;

            var accessToken = await _service.GetAccessTokenAsync();
            var originalRequest = HttpContext.Request;

            var originalPath = HttpContext.Request.Path.ToString();
            var prefix = @"/" + AppSettings.PassThroughPrefix; // value is const, must be gotten from class instead of instance
            var path = HttpContext.Request.Path.ToString().Remove(0, prefix.Length);

            var uri = apiUrl + path;

            var request = (HttpWebRequest)WebRequest.Create(uri + HttpContext.Request.QueryString);

            request.Method = HttpContext.Request.Method;
            request.ContentType = HttpContext.Request.ContentType;

            request.Headers["Authorization"] = "Bearer " + accessToken;
            //1000 * 60 * 10
            request.ContinueTimeout = 600000;

            if (originalRequest.ContentLength.HasValue)
            {
                using (var requestBodyStream = originalRequest.Body)
                {
                    byte[] requestBodyBytes;
                    using (var br = new BinaryReader(requestBodyStream))
                    {
                        requestBodyBytes = br.ReadBytes((int)originalRequest.ContentLength);
                    }

                    using (var requestStream = await request.GetRequestStreamAsync())
                    {
                        await requestStream.WriteAsync(requestBodyBytes, 0, requestBodyBytes.Length);
                        await requestStream.FlushAsync();
                    }
                }
            }

            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    HttpContext.Response.ContentType = response.ContentType;
                    HttpContext.Response.StatusCode = (int)((HttpWebResponse)response).StatusCode;

                    if (response.Headers["x-filename"] != null)
                        HttpContext.Response.Headers["x-filename"] = response.Headers["x-filename"];

                    if (response.Headers["content-disposition"] != null)
                        HttpContext.Response.Headers["content-disposition"] = response.Headers["content-disposition"];

                    using (var responseStream = response.GetResponseStream())
                    {
                        await responseStream.CopyToAsync(HttpContext.Response.Body);
                        await HttpContext.Response.Body.FlushAsync();
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ConnectFailure || ex.Status == WebExceptionStatus.UnknownError)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    return;
                }
                HttpContext.Response.ContentType = ((HttpWebResponse)ex.Response).ContentType;
                HttpContext.Response.StatusCode = (int)((HttpWebResponse)ex.Response).StatusCode;

                using (var responseStream = ex.Response.GetResponseStream())
                {
                    await responseStream.CopyToAsync(HttpContext.Response.Body);
                    await HttpContext.Response.Body.FlushAsync();
                }
            }
        }
    }
}