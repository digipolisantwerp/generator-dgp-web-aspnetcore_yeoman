using System;
using Microsoft.Framework.Logging;
using StarterKit.Utilities.Configs;
using StarterKit.DataAccess.Context;

namespace StarterKit.DataAccess.Uow
{
    public class UowProvider : IUowProvider
    {
        public UowProvider(ILogger logger, IServiceProvider serviceProvider, IDatabaseConfiguration dbConfig)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _dbConfig = dbConfig;
		}

        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDatabaseConfiguration _dbConfig;

        public IUnitOfWork CreateUnitOfWork(bool autoDetectChanges = true, bool enableLogging = false)
        {
			var context = new EntityContext(_dbConfig);
			context.Configuration.AutoDetectChangesEnabled = autoDetectChanges;

			if ( enableLogging )
			{
				context.Database.Log = (msg) => _logger.LogVerbose(msg);		
			}

			var uow = new UnitOfWork(_logger, context, _serviceProvider);
            return uow;
        }
    }
}
