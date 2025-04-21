using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObjects;

namespace Service
{
	public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
	{
		public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
		{
			var repo = _unitOfWork.GetRepository<ProductBrand, int>();
			var brands = await repo.GetAllAsync();
			var brandsDto = _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(brands);
			return brandsDto;
		}

		public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
		{
			var specifications = new ProductWithBrandAndTypeSpecifications(queryParams);
			var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(specifications);
			var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
			var productCount = products.Count();
			return new PaginatedResult<ProductDto>(productCount, queryParams.PageIndex, 0, data);
		}

		public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
		{
			var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
			return _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(types);
		}

		public async Task<ProductDto> GetProductByIdAsync(int id)
		{
			var specifications = new ProductWithBrandAndTypeSpecifications(id);
			var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specifications);
			return _mapper.Map<Product, ProductDto>(product);
		}
	}
}
