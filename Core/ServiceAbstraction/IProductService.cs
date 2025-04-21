using Shared;
using Shared.DataTransferObjects;

namespace ServiceAbstraction
{
	public interface IProductService
	{
		//Get All Products
		Task<IEnumerable<ProductDto>> GetAllProductsAsync(int? brandId , int? typeId , ProductSortingOptions sortingOption);

		//Get Product By Id
		Task<ProductDto> GetProductByIdAsync(int id);

		//Get All Types
		Task<IEnumerable<TypeDto>> GetAllTypesAsync();

		//Get All Brands
		Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
	}
}
