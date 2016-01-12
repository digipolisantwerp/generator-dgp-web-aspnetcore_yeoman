using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Framework.Logging;
using StarterKit.Entities;

namespace StarterKit.DataAccess.Repositories.Base
{
	public abstract class EntityRepositoryBase<TContext, TEntity> : RepositoryBase<TContext>, IRepository<TEntity> where TContext : DbContext where TEntity : EntityBase, new()
	{
		private readonly OrderBy<TEntity> DefaultOrderBy = new OrderBy<TEntity>(qry => qry.OrderBy(e => e.Id));

		protected EntityRepositoryBase(ILogger logger, TContext context) : base(logger, context)
		{ }

		public virtual IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IncludeList<TEntity> includes = null)
		{
			var result = QueryDb(null, orderBy, includes);
			return result.ToList();
		}

		public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IncludeList<TEntity> includes = null)
		{
			var result = QueryDb(null, orderBy, includes);
			return await result.ToListAsync();
		}

		public virtual IEnumerable<TEntity> GetPage(int startRij, int aantal, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IncludeList<TEntity> includes = null)
		{
			if (orderBy == null)
				orderBy = DefaultOrderBy.Expression;

			var result = QueryDb(null, orderBy, includes);
			return result.Skip(startRij).Take(aantal).ToList();
		}

		public virtual async Task<IEnumerable<TEntity>> GetPageAsync(int startRij, int aantal, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IncludeList<TEntity> includes = null)
		{
			if (orderBy == null)
				orderBy = DefaultOrderBy.Expression;

			var result = QueryDb(null, orderBy, includes);
			return await result.Skip(startRij).Take(aantal).ToListAsync();
		}

		public virtual TEntity Get(int id, IncludeList<TEntity> includes = null)
		{
			IQueryable<TEntity> query = Context.Set<TEntity>();

			if (includes != null)
			{
				query = AddIncludes(query, includes);
			}

			return query.SingleOrDefault(x => x.Id == id);
		}

		public virtual async Task<TEntity> GetAsync(int id, IncludeList<TEntity> includes = null)
		{
			IQueryable<TEntity> query = Context.Set<TEntity>();

			if (includes != null)
			{
				query = AddIncludes(query, includes);
			}

			return await query.SingleOrDefaultAsync(x => x.Id == id);
		}

		public virtual IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IncludeList<TEntity> includes = null)
		{
			var result = QueryDb(filter, orderBy, includes);
			return result.ToList();
		}

		public virtual async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IncludeList<TEntity> includes = null)
		{
			var result = QueryDb(filter, orderBy, includes);
			return await result.ToListAsync();
		}

		public virtual IEnumerable<TEntity> QueryPage(int startRij, int aantal, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IncludeList<TEntity> includes = null)
		{
			if (orderBy == null)
				orderBy = DefaultOrderBy.Expression;

			var result = QueryDb(filter, orderBy, includes);
			return result.Skip(startRij).Take(aantal).ToList();
		}

		public virtual async Task<IEnumerable<TEntity>> QueryPageAsync(int startRij, int aantal, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, IncludeList<TEntity> includes = null)
		{
			if (orderBy == null)
				orderBy = DefaultOrderBy.Expression;

			var result = QueryDb(filter, orderBy, includes);
			return await result.Skip(startRij).Take(aantal).ToListAsync();
		}

		public virtual void Add(TEntity entity)
		{
			if (entity == null) throw new ArgumentNullException("entity", "entity is null in repository.Insert.");
			Context.Set<TEntity>().Add(entity);
		}

		public virtual TEntity Update(TEntity entity)
		{
			var bestaande = Context.Set<TEntity>().Find(entity.Id);
			if (bestaande == null) throw new EntityNotFoundException(typeof(TEntity).Name, entity.Id);
			Context.Entry(bestaande).CurrentValues.SetValues(entity);
			return bestaande;
		}

		public virtual void Remove(TEntity entity)
		{
			Context.Set<TEntity>().Remove(entity);
		}

		public virtual void Remove(int id)
		{
			var entity = new TEntity() { Id = id };
			this.Remove(entity);
		}

		public virtual int Count(Expression<Func<TEntity, bool>> filter = null)
		{
			IQueryable<TEntity> query = Context.Set<TEntity>();

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return query.Count();
		}

		public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
		{
			IQueryable<TEntity> query = Context.Set<TEntity>();

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return query.CountAsync();
		}

		protected IQueryable<TEntity> QueryDb(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, IncludeList<TEntity> includes)
		{
			IQueryable<TEntity> query = Context.Set<TEntity>();

			if (filter != null)
			{
				query = query.Where(filter);
			}

			if (includes != null)
			{
				query = AddIncludes(query, includes);
			}

			if (orderBy != null)
			{
				query = orderBy(query);
			}

			return query;
		}

		protected IQueryable<TEntity> AddIncludes(IQueryable<TEntity> query, IncludeList<TEntity> includes)
		{
			if (includes.Includes.Count() > 0)
			{
				foreach (var includeProperty in includes.Includes)
				{
					query = query.Include(includeProperty);
				}
			}
			return query;
		}

	}
}