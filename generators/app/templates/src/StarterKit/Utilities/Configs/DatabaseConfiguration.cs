using Microsoft.Framework.Configuration;

namespace StarterKit.Utilities.Configs
{
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        public DatabaseConfiguration(IConfiguration config)
		{
			_config = config;
		}
		
		private readonly IConfiguration _config;
        
        private string _connectionString;
        public string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                    _connectionString = string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4}",
                                                _config.Get("starterkit:Host"), 
                                                _config.Get("starterkit:Port"), 
                                                _config.Get("starterkit:Name"), 
                                                _config.Get("starterkit:Userid"), 
                                                _config.Get("starterkit:Password"));
                return _connectionString;

            }
        }
    }
}