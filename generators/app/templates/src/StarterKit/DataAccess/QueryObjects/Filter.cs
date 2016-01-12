using System;
using System.Linq.Expressions;
using StarterKit.Entities;
using StarterKit.DataAccess.QueryObjects;

namespace StarterKit.DataAccess
{
	public class Filter<TEntity> where TEntity : EntityBase
	{
		public Filter(Expression<Func<TEntity, bool>> expression)
		{
			Expression = expression;
		}

		public Expression<Func<TEntity, bool>> Expression { get; private set; }

		public void AddExpression(Expression<Func<TEntity, bool>> nieuweExpression)
		{
			if (nieuweExpression == null)
				throw new ArgumentNullException();

			if (Expression == null)
				Expression = nieuweExpression;

			var parameter = System.Linq.Expressions.Expression.Parameter(typeof(TEntity));

			var leftVisitor = new ReplaceExpressionVisitor(nieuweExpression.Parameters[0], parameter);
			var left = leftVisitor.Visit(nieuweExpression.Body);

			var rightVisitor = new ReplaceExpressionVisitor(Expression.Parameters[0], parameter);
			var right = rightVisitor.Visit(Expression.Body);

			Expression = System.Linq.Expressions.Expression.Lambda<Func<TEntity, bool>>(System.Linq.Expressions.Expression.AndAlso(left, right), parameter);
		}
	}
}