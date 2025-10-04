using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Persistence.Identity;
using System.Text.Json;

namespace Persistence
{
	public class DataSeeding(StoreDbContext _dbContext,
							 UserManager<ApplicationUser> _userManager,
							 RoleManager<IdentityRole> _roleManager,
							 StoreIdentityDbContext _identityDbContext) : IDataSeeding
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

				if (!_dbContext.Set<DeliveryMethod>().Any())
				{
					var deliveryMethodData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\delivery.json");
					var deliveryMethods = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(deliveryMethodData);
					if (deliveryMethods is not null && deliveryMethods.Any())
						await _dbContext.Set<DeliveryMethod>().AddRangeAsync(deliveryMethods);
				}

				await _dbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				//TO DO
			}
		}

		public async Task IdentityDataSeedAsync()
		{
			try
			{
				if (!_roleManager.Roles.Any())
				{
					await _roleManager.CreateAsync(new IdentityRole("Admin"));
					await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
				}

				if (!_userManager.Users.Any())
				{
					var user01 = new ApplicationUser()
					{
						Email = "Mohamed@gmail.com",
						DisplayName = "Mohamed Hosam",
						PhoneNumber = "1234567890",
						UserName = "MohamedHosam"
					};

					var user02 = new ApplicationUser()
					{
						Email = "Salma@gmail.com",
						DisplayName = "Salma Hosam",
						PhoneNumber = "1234567890",
						UserName = "SalmaHosam"
					};

					await _userManager.CreateAsync(user01, "P@ssw0rd");
					await _userManager.CreateAsync(user02, "P@ssw0rd");

					await _userManager.AddToRoleAsync(user01, "Admin");
					await _userManager.AddToRoleAsync(user02, "SuperAdmin");
				}

				await _identityDbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{

			}
		}
	}
}
