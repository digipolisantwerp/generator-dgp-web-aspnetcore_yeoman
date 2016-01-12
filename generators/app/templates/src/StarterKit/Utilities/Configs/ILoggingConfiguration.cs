using System;

namespace StarterKit.Utilities.Configs
{
    public interface ILoggingConfiguration
    {
        string Name { get; }
        LoggingFileTargetConfiguration FileTarget { get; }
    }
}