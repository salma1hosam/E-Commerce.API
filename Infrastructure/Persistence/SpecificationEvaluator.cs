using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
	internal static class SpecificationEvaluator
	{
		//Create Query

		//     Entry Point                            Specifications
		//_dbContext.Products.Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType)
		public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> entryPoint, ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
		{
			var Query = entryPoint; //_dbContext.Set<TEntity>()

			if (specifications.Criteria is not null)
				Query = Query.Where(specifications.Criteria);

			if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Count > 0)
			{
				//foreach (var expression in specifications.IncludeExpressions)
				//	Query = Query.Include(expression);

				//OR

				specifications.IncludeExpressions.Aggregate(Query, (currentQuery, includeExp) => currentQuery.Include(includeExp));
			}

			return Query;
		}
	}
}
