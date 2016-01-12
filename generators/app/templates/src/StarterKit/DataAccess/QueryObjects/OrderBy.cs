using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using StarterKit.Entities;

namespace StarterKit.DataAccess
{
    public class OrderBy<TEntity> where TEntity : EntityBase
	{
		public OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> expression)
		{
			Expression = expression;
		}

        public OrderBy(string ordercol, bool reverse)
        {
            Expression = GetOrderBy(ordercol, reverse);
        }

        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> Expression { get; private set; }


        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy(string orderColumn, bool reverse)
        {
            Type typeQueryable = typeof(IQueryable<TEntity>);
            ParameterExpression argQueryable = System.Linq.Expressions.Expression.Parameter(typeQueryable, "p");
            var outerExpression = System.Linq.Expressions.Expression.Lambda(argQueryable, argQueryable);
            string[] props = orderColumn.Split('.');
            IQueryable<TEntity> query = new List<TEntity>().AsQueryable<TEntity>();
            Type type = typeof(TEntity);
            ParameterExpression arg = System.Linq.Expressions.Expression.Parameter(type, "x");

            System.Linq.Expressions.Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                expr = System.Linq.Expressions.Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            LambdaExpression lambda = System.Linq.Expressions.Expression.Lambda(expr, arg);
            string methodName = reverse ? "OrderByDescending" : "OrderBy";

            MethodCallExpression resultExp =
                System.Linq.Expressions.Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TEntity), type }, outerExpression.Body, System.Linq.Expressions.Expression.Quote(lambda));
            var finalLambda = System.Linq.Expressions.Expression.Lambda(resultExp, argQueryable);
            return (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)finalLambda.Compile();
        }
    }
}