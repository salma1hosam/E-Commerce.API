using DomainLayer.Contracts;
using DomainLayer.Models;
using Persistence.Data;

namespace Persistence.Repositories
{
	public class UnitOfWork(StoreDbContext _dbContext) : IUnitOfWork
	{
		private readonly Dictionary<string, object> _repositories = [];
		public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
		{
			//1.Get Type (TEntity) Name
			var typeName = typeof(TEntity).Name;

			//2.Dictionary<string,object> => string: Name of Type [Key] , object: object from Generic Repository [Value]

			//3.if the Type is already created before (have an object from it) , return this object
			//if (_repositories.ContainsKey(typeName))
			//	return (IGenericRepository<TEntity, TKey>)_repositories[typeName];
			//OR
			if (_repositories.TryGetValue(typeName, out object? value))
				return (IGenericRepository<TEntity, TKey>)value;

			//4.else, Create an object from it
			else
			{
				//5.Create the Object
				var repository = new GenericRepository<TEntity, TKey>(_dbContext);

				//6.Store the Object in the Dictionary
				_repositories["typeName"] = repository;

				//7.Return the Object
				return repository;
			}
		}

		public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
	}
}
