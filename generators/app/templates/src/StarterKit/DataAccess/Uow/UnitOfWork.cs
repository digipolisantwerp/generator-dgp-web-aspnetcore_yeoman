using System;
using Microsoft.Framework.Logging;
using StarterKit.DataAccess.Context;

namespace StarterKit.DataAccess.Uow
{
    public class UnitOfWork : UnitOfWorkBase<EntityContext>, IUnitOfWork
    {
        public UnitOfWork(ILogger logger, EntityContext context, IServiceProvider provider) : base(logger, context, provider)
        { }
    }
}
