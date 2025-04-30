using Shared;
using Shared.DataTransferObjects.ProductModule;

namespace ServiceAbstraction
{
    public interface IProductService
	{
		//Get All Products
		Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams);

		//Get Product By Id
		Task<ProductDto> GetProductByIdAsync(int id);

		//Get All Types
		Task<IEnumerable<TypeDto>> GetAllTypesAsync();

		//Get All Brands
		Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
	}
}
