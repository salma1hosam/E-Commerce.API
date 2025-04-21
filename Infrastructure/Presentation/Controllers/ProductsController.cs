using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObjects;

namespace Presentation.Controllers
{
	[ApiController]
	[Route("api/[Controller]")] // BaseUrl/api/Products
	public class ProductsController(IServiceManager _serviceManager) : ControllerBase
	{
		//Get All Products
		[HttpGet] //GET BaseUrl/api/Products
		public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
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
