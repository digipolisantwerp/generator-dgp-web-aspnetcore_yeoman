using System;
using Microsoft.Framework.Logging;
using StarterKit.DataAccess.Context;
using StarterKit.Entities;

namespace StarterKit.DataAccess.Repositories.Base
{
    public class GenericEntityRepository<TEntity> : EntityRepositoryBase<EntityContext, TEntity> where TEntity : EntityBase, new()
    {
		public GenericEntityRepository(ILogger logger) : base(logger, null)
		{ }
	}
}