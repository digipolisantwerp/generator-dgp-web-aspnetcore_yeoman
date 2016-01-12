using System.Threading.Tasks;
using StarterKit.DataAccess;
using StarterKit.Entities;

namespace StarterKit
{
	public interface IPager<TEntity> where TEntity : EntityBase
    {
		Page<TEntity> GetAll(int pagina, int aantal, OrderBy<TEntity> orderby = null, IncludeList<TEntity> includes = null);
		Page<TEntity> Query(int pagina, int aantal, Filter<TEntity> filter, OrderBy<TEntity> orderby = null, IncludeList<TEntity> includes = null);

		Task<Page<TEntity>> GetAllAsync(int pagina, int aantal, OrderBy<TEntity> orderby = null, IncludeList<TEntity> includes = null);
		Task<Page<TEntity>> QueryAsync(int pagina, int aantal, Filter<TEntity> filter, OrderBy<TEntity> orderby = null, IncludeList<TEntity> includes = null);
	}
}