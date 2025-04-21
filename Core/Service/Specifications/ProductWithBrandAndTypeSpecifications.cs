using DomainLayer.Models;

namespace Service.Specifications
{
	internal class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product, int>
	{
		//Get All Products With Brand and Type
		public ProductWithBrandAndTypeSpecifications(int? brandId, int? typeId)
			  : base(P => (!brandId.HasValue || P.ProductBrand.Id == brandId)
					 && (!typeId.HasValue || P.ProductType.Id == typeId))  //Where(P => P.ProductBrand.Id == brandId && P.ProductType.Id == typeId)
		{
			AddInclude(P => P.ProductBrand);
			AddInclude(P => P.ProductType);
		}

		//Get Product By Id With Brand and Type
		public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
		{
			AddInclude(P => P.ProductBrand);
			AddInclude(P => P.ProductType);
		}
	}
}
