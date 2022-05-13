using System;
using Digipolis.Auth.Options;

namespace FOOBAR.Shared.Options
{
  public class AuthSettingsConfig
  {
    public static void SetConfig(AuthOptions authOptions, AppSettings appSettings = null)
    {
      var env = Environment.GetEnvironmentVariables();

      // *** general ***
      //It should be sufficient to set the api key in appsettings and use it here. no reason to define it twice...
      //if an api key was defined in the environments it should take precedence over the api key defined in the json file.
      var apiKeyFallback = appSettings?.ApiKey ?? authOptions.ApiKey;
      authOptions.ApiKey = env.Contains("AUTH_APIKEY") ? env["AUTH_APIKEY"]?.ToString() : apiKeyFallback;
      var applicationNameFallback = appSettings?.AppName ?? authOptions.ApplicationName;
      authOptions.ApplicationName = env.Contains("AUTH_APPLICATIONNAME")
        ? env["AUTH_APPLICATIONNAME"].ToString()
        : applicationNameFallback;
      authOptions.ApplicationBaseUrl = env.Contains("AUTH_APPLICATIONBASEURL")
        ? env["AUTH_APPLICATIONBASEURL"].ToString()
        : authOptions.ApplicationBaseUrl;
      authOptions.PdpUrl = env.Contains("AUTH_PDPURL") ? env["AUTH_PDPURL"].ToString() : authOptions.PdpUrl;
      authOptions.PdpCacheDuration = env.Contains("AUTH_PDPCACHEDURATION")
        ? Convert.ToInt32(env["AUTH_PDPCACHEDURATION"].ToString())
        : authOptions.PdpCacheDuration;
      authOptions.PermissionSource = env.Contains("AUTH_PERMISSIONSOURCE")
        ? env["AUTH_PERMISSIONSOURCE"].ToString()
        : authOptions.PermissionSource;
      authOptions.LoadPDPPermissions = env.Contains("AUTH_LOADPDPPERMISSIONS")
        ? Convert.ToBoolean(env["AUTH_LOADPDPPERMISSIONS"].ToString())
        : authOptions.LoadPDPPermissions;
      authOptions.LoadPDPRoles = env.Contains("AUTH_LOADPDPROLES")
        ? Convert.ToBoolean(env["AUTH_LOADPDPROLES"].ToString())
        : authOptions.LoadPDPPermissions;
      authOptions.TokenRefreshUrl = env.Contains("AUTH_TOKENREFRESHURL")
        ? env["AUTH_TOKENREFRESHURL"].ToString()
        : authOptions.TokenRefreshUrl;
      authOptions.LogoutUrl = env.Contains("AUTH_LOGOUTURL") ? env["AUTH_LOGOUTURL"].ToString() : authOptions.LogoutUrl;

      // *** endpoints ***
      authOptions.AuthorizationEndpoint = env.Contains("AUTH_AUTHORIZATIONENDPOINT")
        ? env["AUTH_AUTHORIZATIONENDPOINT"].ToString()
        : authOptions.AuthorizationEndpoint;
      authOptions.UserinfoEndpoint = env.Contains("AUTH_USERINFOENDPOINT")
        ? env["AUTH_USERINFOENDPOINT"].ToString()
        : authOptions.UserinfoEndpoint;
      authOptions.MeAuthzUrl =
        env.Contains("AUTH_MEAUTHZURL") ? env["AUTH_MEAUTHZURL"].ToString() : authOptions.MeAuthzUrl;

      // ***OAuth options ***
      authOptions.EnableOAuth = env.Contains("AUTH_ENABLEOAUTH")
        ? Convert.ToBoolean(env["AUTH_ENABLEOAUTH"].ToString())
        : authOptions.EnableOAuth;
      authOptions.OAuthKeyStore = env.Contains("AUTH_OAUTHKEYSTORE")
        ? env["AUTH_OAUTHKEYSTORE"].ToString()
        : authOptions.OAuthKeyStore;
      authOptions.OAuthKeyStoreDatabase = env.Contains("AUTH_OAUTHKEYSTOREDATABASE")
        ? env["AUTH_OAUTHKEYSTOREDATABASE"].ToString()
        : authOptions.OAuthKeyStoreDatabase;
      authOptions.ResponseType = env.Contains("AUTH_RESPONSETYPE")
        ? env["AUTH_RESPONSETYPE"].ToString()
        : authOptions.ResponseType;
      authOptions.ClientId = env.Contains("AUTH_CLIENTID") ? env["AUTH_CLIENTID"].ToString() : authOptions.ClientId;
      authOptions.ClientSecret = env.Contains("AUTH_CLIENTSECRET")
        ? env["AUTH_CLIENTSECRET"].ToString()
        : authOptions.ClientSecret;
      authOptions.Scope = env.Contains("AUTH_SCOPE") ? env["AUTH_SCOPE"].ToString() : authOptions.Scope;
      authOptions.RedirectUri =
        env.Contains("AUTH_REDIRECTURI") ? env["AUTH_REDIRECTURI"].ToString() : authOptions.RedirectUri;
      //authOptions.State --- not used yet

      // *** Digipolis specific ***
      //authOptions.RedirectUriLng --- not used yet
      authOptions.Service = env.Contains("AUTH_SERVICE") ? env["AUTH_SERVICE"].ToString() : authOptions.Service;
      authOptions.Language = env.Contains("AUTH_LANGUAGE") ? env["AUTH_LANGUAGE"].ToString() : authOptions.Language;
      authOptions.ForceAuth = env.Contains("AUTH_FORCEAUTH")
        ? Convert.ToBoolean(env["AUTH_FORCEAUTH"].ToString())
        : authOptions.ForceAuth;
      authOptions.SaveConsent = env.Contains("AUTH_SAVECONSENT")
        ? Convert.ToBoolean(env["AUTH_SAVECONSENT"].ToString())
        : authOptions.SaveConsent;
      authOptions.OAuthCookieLifeTimeInSeconds = env.Contains("AUTH_OAUTHCOOKIELIFETIMEINSECONDS")
        ? Convert.ToInt32(env["AUTH_OAUTHCOOKIELIFETIMEINSECONDS"].ToString())
        : authOptions.OAuthCookieLifeTimeInSeconds;


      // *** provided routes ***
      authOptions.PermissionsRoute = env.Contains("AUTH_PERMISSIONSROUTE")
        ? env["AUTH_PERMISSIONSROUTE"].ToString()
        : authOptions.PermissionsRoute;
      authOptions.UserRoute = env.Contains("AUTH_USERROUTE") ? env["AUTH_USERROUTE"].ToString() : authOptions.UserRoute;

      // *** other options ***
      authOptions.AccessDeniedRoute = env.Contains("AUTH_ACCESSDENIEDROUTE")
        ? env["AUTH_ACCESSDENIEDROUTE"].ToString()
        : authOptions.AccessDeniedRoute;

      authOptions.SuppressFetchUserProfileFailedException = env.Contains("AUTH_SUPPRESSFETCHUSERPROFILEFAILEDEXCEPTION")
        ? Convert.ToBoolean(env["AUTH_SUPPRESSFETCHUSERPROFILEFAILEDEXCEPTION"].ToString())
        : authOptions.SuppressFetchUserProfileFailedException;
    }
  }
}
