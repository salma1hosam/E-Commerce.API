using DomainLayer.Models;
using System.Linq.Expressions;

namespace DomainLayer.Contracts
{
	public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
	{
		//Property Signature of Each Dynamic Part in the Query (Where Expression & Include Expressions)
		public Expression<Func<TEntity, bool>>? Criteria { get; } //Where Expression
		public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } //Include Expressions
	}
}
