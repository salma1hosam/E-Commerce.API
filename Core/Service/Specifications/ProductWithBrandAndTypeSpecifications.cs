using DomainLayer.Models;
using Shared;

namespace Service.Specifications
{
	internal class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product, int>
	{
		//Get All Products With Brand and Type
		public ProductWithBrandAndTypeSpecifications(int? brandId, int? typeId, ProductSortingOptions sortingOption)
			  : base(P => (!brandId.HasValue || P.ProductBrand.Id == brandId)
					 && (!typeId.HasValue || P.ProductType.Id == typeId))  //Where(P => P.ProductBrand.Id == brandId && P.ProductType.Id == typeId)
		{
			AddInclude(P => P.ProductBrand);
			AddInclude(P => P.ProductType);

			switch (sortingOption)
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
		}

		//Get Product By Id With Brand and Type
		public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
		{
			AddInclude(P => P.ProductBrand);
			AddInclude(P => P.ProductType);
		}
	}
}
