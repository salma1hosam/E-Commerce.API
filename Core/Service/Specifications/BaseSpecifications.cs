using DomainLayer.Contracts;
using DomainLayer.Models;
using System.Linq.Expressions;

namespace Service.Specifications
{
	internal abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
	{
		//Any class will inherit from BaseSpecifications class must Chain on this constructor so, must pass the Where Expression (will set it through the Constructor)
		protected BaseSpecifications(Expression<Func<TEntity, bool>> criteriaExpression)
		{
			Criteria = criteriaExpression;
		}
		public Expression<Func<TEntity, bool>> Criteria { get; private set; }

		public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

		//Will Set or Add an Include Expression in the IncludeExpressions List through the function
		protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
			=> IncludeExpressions.Add(includeExpression);
	}
}
