using DomainLayer.Models.ProductModule;
using Shared;

namespace Service.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product, int>
	{
		//Get All Products With Brand and Type
		public ProductWithBrandAndTypeSpecifications(ProductQueryParams queryParams)
			  : base(P => (!queryParams.BrandId.HasValue || P.ProductBrand.Id == queryParams.BrandId)
					 && (!queryParams.TypeId.HasValue || P.ProductType.Id == queryParams.TypeId) //Where(P => P.ProductBrand.Id == brandId && P.ProductType.Id == typeId)
					 && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
		{
			AddInclude(P => P.ProductBrand);
			AddInclude(P => P.ProductType);

			switch (queryParams.SortingOption)
			{
				case ProductSortingOptions.NameAsc:
					AddOrderBy(P => P.Name);
					break;
				case ProductSortingOptions.NameDesc:
					AddOrderByDescending(P => P.Name);
					break;
				case ProductSortingOptions.PriceAsc:
					AddOrderBy(P => P.Price);
					break;
				case ProductSortingOptions.PriceDesc:
					AddOrderByDescending(P => P.Price);
					break;
				default:
					break;
			}

			ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
		}

		//Get Product By Id With Brand and Type
		public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
		{
			AddInclude(P => P.ProductBrand);
			AddInclude(P => P.ProductType);
		}
	}
}
