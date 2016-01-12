using Microsoft.Framework.Configuration;

namespace StarterKit.Utilities.Configs
{
    public class LoggingConfiguration : ILoggingConfiguration
    {
        public LoggingConfiguration(IConfiguration config)
		{
			_config = config;
		}
		
		private readonly IConfiguration _config;
        
        private string _name;
        public string Name
        {
            get
            {
                if ( _name == null )
                    _name = _config.Get("Name");
                return _name;
            }
        }

        private LoggingFileTargetConfiguration _fileTarget;
        public LoggingFileTargetConfiguration FileTarget
        {
            get
            {
                if ( _fileTarget == null )
                {
                    _fileTarget = new LoggingFileTargetConfiguration()
                    {
                        FileName = _config.Get("FileTarget:FileName"),
                        Layout = _config.Get("FileTarget:Layout"),
                        Level = _config.Get("FileTarget:Level"),
                        Name = _config.Get("FileTarget:Name"),
						Path = _config.Get("FileTarget:Path")
                    };
                }
                return _fileTarget;
            }
        }
    }
}