using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Text.Json;

namespace Persistence
{
	public class DataSeeding(StoreDbContext _dbContext) : IDataSeeding
	{
		public async Task DataSeedAsync()
		{
			try
			{
				//Production
				var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
				if (pendingMigrations.Any())
					await _dbContext.Database.MigrateAsync(); //Apply all the unapplied Migrations (Update-Database)

				if (!_dbContext.ProductBrands.Any())
				{
					//var productBrandData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");  //return string
					var productBrandData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");  //return file Stream

					//Convert Data "string" => C# Objects [ProductBrand]
					var productBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(productBrandData); //the Async Version of Deserialize() method takes a stream as a parameter not a string

					if (productBrands is not null && productBrands.Any())
						await _dbContext.ProductBrands.AddRangeAsync(productBrands);
				}

				if (!_dbContext.ProductTypes.Any())
				{
					var productTypeData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\types.json");
					var productTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(productTypeData);
					if (productTypeData is not null && productTypes.Any())
						await _dbContext.ProductTypes.AddRangeAsync(productTypes);
				}

				if (!_dbContext.Products.Any())
				{
					var productData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\products.json");
					var products = await JsonSerializer.DeserializeAsync<List<Product>>(productData);
					if (products is not null && products.Any())
						await _dbContext.Products.AddRangeAsync(products);
				}

				await _dbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				//TO DO
			}
		}
	}
}
