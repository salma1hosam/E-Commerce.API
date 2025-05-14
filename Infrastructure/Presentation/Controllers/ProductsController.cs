using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObjects.ProductModule;

namespace Presentation.Controllers
{
	public class ProductsController(IServiceManager _serviceManager) : ApiBaseController
	{
		//Get All Products
		//[Authorize(Roles = "Admin")]
		[HttpGet] //GET BaseUrl/api/Products
		public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
		{
			var products = await _serviceManager.ProductService.GetAllProductsAsync(queryParams);
			return Ok(products);
		}

		//Get Product By Id
		[HttpGet("{id:int}")] //GET BaseUrl/api/Products/10
		public async Task<ActionResult<ProductDto>> GetProduct(int id)
		{
			var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
			return Ok(product);
		}

		//Get All Types
		[HttpGet("types")] //GET BaseUrl/api/Products/types
		public async Task<ActionResult<IEnumerable<TypeDto>>> GetTypes()
		{
			var types = await _serviceManager.ProductService.GetAllTypesAsync();
			return Ok(types);
		}

		//Get All Brands
		[HttpGet("brands")] //GET BaseUrl/api/Products/brands
		public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
		{
			var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
			return Ok(brands);
		}
	}
}
