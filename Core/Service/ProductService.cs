using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.ProductModule;
using Service.Specifications;
using Service.Specifications.ProductModuleSpecifications;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObjects.ProductModule;

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
			var repository = _unitOfWork.GetRepository<Product, int>();
			var specifications = new ProductWithBrandAndTypeSpecifications(queryParams);
			var products = await repository.GetAllAsync(specifications);
			var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
			var productCount = products.Count();
			var countSpecifications = new ProductCountSpecification(queryParams);
			var totalCount = await repository.CountAsync(countSpecifications);
			return new PaginatedResult<ProductDto>(productCount, queryParams.PageNumber, totalCount, data);
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
			if (product is null)
				throw new ProductNotFoundException(id);
			return _mapper.Map<Product, ProductDto>(product);
		}
	}
}
