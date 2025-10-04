
namespace Shared
{
	public class ProductQueryParams
	{
		private const int defaultPageSize = 5;
		private const int maxPageSize = 10;

		public int? BrandId { get; set; }
		public int? TypeId { get; set; }
		public ProductSortingOptions Sort { get; set; }
		public string? Search { get; set; }
		public int PageNumber { get; set; } = 1;

		private int pageSize = defaultPageSize;
		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value > maxPageSize ? maxPageSize : value; }
		}

	}
}
