using System.Threading.Tasks;
using StarterKit.DataAccess;
using StarterKit.DataAccess.Repositories;
using StarterKit.Entities;

namespace StarterKit
{
	public class Pager<TEntity> : IPager<TEntity> where TEntity : EntityBase
    {
		public Pager(IUowProvider uowProvider)
		{
			_uowProvider = uowProvider;
		}

		private readonly IUowProvider _uowProvider;

		public Page<TEntity> GetAll(int pagina, int aantal, OrderBy<TEntity> orderby = null, IncludeList<TEntity> includes = null)
		{
			using (var uow = _uowProvider.CreateUnitOfWork(false))
			{
				var repository = uow.GetRepository<IRepository<TEntity>>();

				var startRij = ( pagina - 1 ) * aantal;
				var page = new Page<TEntity>()
				{
					Lijst = repository.GetPage(startRij, aantal, includes: includes, orderBy: orderby?.Expression),
					TotaalAantal = repository.Count()
				};

				return page;
			}
		}

		public async Task<Page<TEntity>> GetAllAsync(int pagina, int aantal, OrderBy<TEntity> orderby = null, IncludeList<TEntity> includes = null)
		{
			using (var uow = _uowProvider.CreateUnitOfWork(false))
			{
				var repository = uow.GetRepository<IRepository<TEntity>>();

				var startRij = ( pagina - 1 ) * aantal;
				var page = new Page<TEntity>()
				{
					Lijst = await repository.GetPageAsync(startRij, aantal, includes: includes, orderBy: orderby?.Expression),
					TotaalAantal = await repository.CountAsync()
				};

				return page;
			}
		}

		public Page<TEntity> Query(int pagina, int aantal, Filter<TEntity> filter, OrderBy<TEntity> orderby = null, IncludeList<TEntity> includes = null)
		{
			using (var uow = _uowProvider.CreateUnitOfWork(false))
			{
				var repository = uow.GetRepository<IRepository<TEntity>>();

                var startRij = (pagina - 1) * aantal;
                var page = new Page<TEntity>()
				{
					Lijst = repository.QueryPage(startRij, aantal, filter.Expression, includes: includes, orderBy: orderby?.Expression),
					TotaalAantal = repository.Count(filter.Expression)
				};

				return page;
			}
		}

		public async Task<Page<TEntity>> QueryAsync(int pagina, int aantal, Filter<TEntity> filter, OrderBy<TEntity> orderby = null, IncludeList<TEntity> includes = null)
		{
			using (var uow = _uowProvider.CreateUnitOfWork(false))
			{
				var repository = uow.GetRepository<IRepository<TEntity>>();

				var startRij = ( pagina - 1 ) * aantal;
				var page = new Page<TEntity>()
				{
					Lijst = await repository.QueryPageAsync(startRij, aantal, filter.Expression, includes: includes, orderBy: orderby?.Expression),
					TotaalAantal = await repository.CountAsync(filter.Expression)
				};

				return page;
			}
		}
	}
}