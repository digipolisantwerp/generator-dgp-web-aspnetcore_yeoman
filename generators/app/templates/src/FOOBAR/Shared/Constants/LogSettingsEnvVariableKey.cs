namespace FOOBAR.Shared.Constants
{
  /// <summary>
  /// keys for LogSettings environment variables
  /// </summary>
  public class LogSettingsEnvVariableKey
  {
    // GENERAL
    public const string LogExceptions = "LOGSETTINGS_LOGEXCEPTIONS";

    // SERILOG
    public const string SeriLog_MinimumLevel_Default = "LOGSETTINGS_SERILOG_MINIMUMLEVEL_DEFAULT";
    public const string SeriLog_MinimumLevel_Override_System = "LOGSETTINGS_SERILOG_MINIMUMLEVEL_OVERRIDE_SYSTEM";
    public const string SeriLog_MinimumLevel_Override_Microsoft = "LOGSETTINGS_SERILOG_MINIMUMLEVEL_OVERRIDE_MICROSOFT";

    // REQUEST LOGGING - GENERAL
    public const string RequestLogging_LogPayload = "LOGSETTINGS_REQUESTLOGGING_LOGPAYLOAD";
    public const string RequestLogging_LogPayloadOnError = "LOGSETTINGS_REQUESTLOGGING_LOGPAYLOADONERROR";

    // REQUEST LOGGING - INCOMING (middleware)
    public const string RequestLogging_Incoming_Enabled = "LOGSETTINGS_REQUESTLOGGING_INCOMING_ENABLED";

    public const string RequestLogging_Incoming_AllowedRequestHeaders =
      "LOGSETTINGS_REQUESTLOGGING_INCOMING_ALLOWEDREQUESTHEADERS";

    public const string RequestLogging_Incoming_AllowedResponseHeaders =
      "LOGSETTINGS_REQUESTLOGGING_INCOMING_ALLOWEDRESPONSEHEADERS";

    // REQUEST LOGGING - OUTGOING (delegating handler)
    public const string RequestLogging_OutgoingEnabled = "LOGSETTINGS_REQUESTLOGGING_OUTGOING_ENABLED";

    public const string RequestLogging_Outgoing_AllowedRequestHeaders =
      "LOGSETTINGS_REQUESTLOGGING_OUTGOING_ALLOWEDREQUESTHEADERS";

    public const string RequestLogging_Outgoing_AllowedResponseHeaders =
      "LOGSETTINGS_REQUESTLOGGING_OUTGOING_ALLOWEDRESPONSEHEADERS";
  }
}


/*
"logsettings": {
  "logexceptions": "true",
  "serilog": {
    "minimumlevel": {
      "default": "Information",
      "override": {
        "system": "Error",
        "microsoft": "Error"
      }
    }
  },
  "requestlogging": {
    "incoming": {
      "enabled": "true",
      "allowedRequestHeaders": "",    // omit to use defaults
      "allowedResponseHeaders": ""    // omit to use defaults
    },
    "outgoing": {
      "enabled": "true",
      "allowedRequestHeaders": "",    // omit to use defaults
      "allowedResponseHeaders": ""    // omit to use defaults
    }
  }
}
 
 */
