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

			if (specifications.OrderBy is not null)
				Query = Query.OrderBy(specifications.OrderBy);

			if (specifications.OrderByDescending is not null)
				Query = Query.OrderByDescending(specifications.OrderByDescending);

			if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Count > 0)
			{
				//foreach (var expression in specifications.IncludeExpressions)
				//	Query = Query.Include(expression);

				//OR

				Query = specifications.IncludeExpressions.Aggregate(Query, (currentQuery, includeExp) => currentQuery.Include(includeExp));
				//currentQuery = _dbContext.Products.Where(P => P.Id == id)
				//includeExp = P => P.ProductBrand
				//currentQuery = _dbContext.Products.Where(P => P.Id == id).Include(P => P.ProductBrand)
				//includeExp = P => P.ProductType
				//currentQuery = _dbContext.Products.Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType)

			}

			if (specifications.IsPaginated)
				Query = Query.Skip(specifications.Skip).Take(specifications.Take);

			return Query;
		}
	}
}
