using DomainLayer.Models;
using Shared;

namespace Service.Specifications
{
	internal class ProductCountSpecification : BaseSpecifications<Product, int>
	{
		public ProductCountSpecification(ProductQueryParams queryParams)
			: base(P => (!queryParams.BrandId.HasValue || P.ProductBrand.Id == queryParams.BrandId)
					 && (!queryParams.TypeId.HasValue || P.ProductType.Id == queryParams.TypeId) //Where(P => P.ProductBrand.Id == brandId && P.ProductType.Id == typeId)
					 && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
		{
		}
	}
}
