
namespace Shared
{
	public class ProductQueryParams
	{
		public int? BrandId { get; set; }
		public int? TypeId { get; set; }
		public ProductSortingOptions SortingOption { get; set; }
		public string? SearchValue { get; set; }
	}
}
