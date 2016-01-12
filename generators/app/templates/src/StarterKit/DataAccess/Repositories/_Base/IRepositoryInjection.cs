using System.Data.Entity;

namespace StarterKit.DataAccess.Repositories.Base
{
    public interface IRepositoryInjection<TContext> where TContext : DbContext
    {
        IRepositoryInjection<TContext> SetContext(TContext context);
    }
}