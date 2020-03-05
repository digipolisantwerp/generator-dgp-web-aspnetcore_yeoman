using Digipolis.Authentication.OAuth.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace FOOBAR.Shared.Options
{
    public class AuthSettingsConfig
    {
        public static void SetConfig(OAuthOptions authOptions, IOptions<AppSettings> appSetings = null)
        {
            var env = Environment.GetEnvironmentVariables();

            // *** endpoints ***
            authOptions.AuthorizationEndpoint = env.Contains("AUTH_AUTHORIZATIONENDPOINT") ? env["AUTH_AUTHORIZATIONENDPOINT"].ToString() : authOptions.AuthorizationEndpoint;
            authOptions.TokenEndpoint = env.Contains("AUTH_TOKENENDPOINT") ? env["AUTH_TOKENENDPOINT"].ToString() : authOptions.TokenEndpoint;
            authOptions.UserinfoEndpoint = env.Contains("AUTH_USERINFOENDPOINT") ? env["AUTH_USERINFOENDPOINT"].ToString() : authOptions.UserinfoEndpoint;
            authOptions.UserPermissionsEndpoint = env.Contains("AUTH_USERPERMISSIONSENDPOINT") ? env["AUTH_USERPERMISSIONSENDPOINT"].ToString() : authOptions.UserPermissionsEndpoint;

            // ***OAuth options ***

            authOptions.ResponseType = env.Contains("AUTH_RESPONSETYPE") ? env["AUTH_RESPONSETYPE"].ToString() : authOptions.ResponseType;
            authOptions.ClientId = env.Contains("AUTH_CLIENTID") ? env["AUTH_CLIENTID"].ToString() : authOptions.ClientId;
            authOptions.ClientSecret = env.Contains("AUTH_CLIENTSECRET") ? env["AUTH_CLIENTSECRET"].ToString() : authOptions.ClientSecret;
            authOptions.Scope = env.Contains("AUTH_SCOPE") ? env["AUTH_SCOPE"].ToString() : authOptions.Scope;
            authOptions.RedirectUri = env.Contains("AUTH_REDIRECTURI") ? env["AUTH_REDIRECTURI"].ToString() : authOptions.RedirectUri;
            //authOptions.State --- not used yet

            // *** Digipolis specific ***

            //authOptions.RedirectUriLng --- not used yet
            authOptions.Service = env.Contains("AUTH_SERVICE") ? env["AUTH_SERVICE"].ToString() : authOptions.Service;
            authOptions.Language = env.Contains("AUTH_LANGUAGE") ? env["AUTH_LANGUAGE"].ToString() : authOptions.Language;
            authOptions.ForceAuth = env.Contains("AUTH_FORCEAUTH") ? Convert.ToBoolean(env["AUTH_FORCEAUTH"].ToString()) : authOptions.ForceAuth;
            authOptions.SaveConsent = env.Contains("AUTH_SAVECONSENT") ? Convert.ToBoolean(env["AUTH_SAVECONSENT"].ToString()) : authOptions.SaveConsent;
            authOptions.CookieLifeTimeInSeconds = env.Contains("AUTH_COOKIELIFETIMEINSECONDS") ? Convert.ToInt32(env["AUTH_COOKIELIFETIMEINSECONDS"].ToString()) : authOptions.CookieLifeTimeInSeconds;

            //It should be sufficient to set the api key in appsettings and use it here. no reason to define it twice...
            //if an api key was defined in the environments it should take precedence over the api key defined in the json file.
            var apiKeyFallback = appSetings?.Value?.ApiKey ?? authOptions.ApiKey;
            authOptions.ApiKey = env.Contains("AUTH_APIKEY") ? env["AUTH_APIKEY"].ToString() : apiKeyFallback;

            // *** provided routes ***
            authOptions.PermissionsRoute = env.Contains("AUTH_PERMISSIONSROUTE") ? env["AUTH_PERMISSIONSROUTE"].ToString() : authOptions.PermissionsRoute;
            authOptions.UserRoute = env.Contains("AUTH_USERROUTE") ? env["AUTH_USERROUTE"].ToString() : authOptions.UserRoute;

            // *** other options ***
            authOptions.AccessDeniedRoute = env.Contains("AUTH_ACCESSDENIEDROUTE") ? env["AUTH_ACCESSDENIEDROUTE"].ToString() : authOptions.AccessDeniedRoute;
            authOptions.UserInfoAndPermissionsCacheDuration = env.Contains("AUTH_CACHEDURATION") ? Convert.ToInt32(env["AUTH_CACHEDURATION"].ToString()) : authOptions.UserInfoAndPermissionsCacheDuration;

            authOptions.SuppressFetchUserProfileFailedException = env.Contains("AUTH_SUPPRESSFETCHUSERPROFILEFAILEDEXCEPTION") ? Convert.ToBoolean(env["AUTH_SUPPRESSFETCHUSERPROFILEFAILEDEXCEPTION"].ToString()) : authOptions.SuppressFetchUserProfileFailedException;
        }
    }
}