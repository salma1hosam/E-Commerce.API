using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Text.Json;

namespace Persistence
{
	public class DataSeeding(StoreDbContext _dbContext) : IDataSeeding
	{
		public void DataSeed()
		{
			try
			{
				if (_dbContext.Database.GetPendingMigrations().Any())
					_dbContext.Database.Migrate(); //Apply all the unapplied Migrations (Update-Database)

				if (!_dbContext.ProductBrands.Any())
				{
					var productBrandData = File.ReadAllText(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");

					//Convert Data "string" => C# Objects [ProductBrand]
					var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(productBrandData);

					if (productBrands is not null && productBrands.Any())
						_dbContext.ProductBrands.AddRange(productBrands);
				}

				if (!_dbContext.ProductTypes.Any())
				{
					var productTypeData = File.ReadAllText(@"..\Infrastructure\Persistence\Data\DataSeed\types.json");
					var productTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypeData);
					if (productTypeData is not null && productTypes.Any())
						_dbContext.ProductTypes.AddRange(productTypes);
				}

				if (!_dbContext.Products.Any())
				{
					var productData = File.ReadAllText(@"..\Infrastructure\Persistence\Data\DataSeed\products.json");
					var products = JsonSerializer.Deserialize<List<Product>>(productData);
					if (products is not null && products.Any())
						_dbContext.Products.AddRange(products);
				}

				_dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				//TO DO
			}
		}
	}
}
