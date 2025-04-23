using DomainLayer.Contracts;
using DomainLayer.Models;
using System.Linq.Expressions;

namespace Service.Specifications
{
	internal abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
	{
		//Any class will inherit from BaseSpecifications class must Chain on this constructor so, must pass the Where Expression (will set it through the Constructor)
		protected BaseSpecifications(Expression<Func<TEntity, bool>>? criteriaExpression)
		{
			Criteria = criteriaExpression;
		}
		public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

		#region Include
		public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

		//Will Set or Add an Include Expression in the IncludeExpressions List through the function
		protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
			=> IncludeExpressions.Add(includeExpression);
		#endregion

		#region Sorting
		public Expression<Func<TEntity, object>> OrderBy { get; private set; }

		public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

		protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) => OrderBy = orderByExpression;
		protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression) => OrderByDescending = orderByDescendingExpression;
		#endregion

		#region Pagination
		public int Skip { get; private set; }

		public int Take { get; private set; }

		public bool IsPaginated { get; set; }

		protected void ApplyPagination(int pageSize, int pageIndex)
		{
			//Total Count = 40
			//Page Size = 10
			//Page Index = 3
			//10 , 10 , 10 , 10

			IsPaginated = true;
			Take = pageSize;
			Skip = (pageIndex - 1) * pageSize;
		}
		#endregion
	}
}
