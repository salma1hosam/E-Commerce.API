
namespace Shared
{
	public class PaginatedResult<TEntity>
	{
		public PaginatedResult(int pageSize, int pageIndex, int totalCount, IEnumerable<TEntity> data)
		{
			PageSize = pageSize;
			PageIndex = pageIndex;
			Count = totalCount;
			Data = data;
		}

		public int PageSize { get; set; }
		public int PageIndex { get; set; }
		public int Count { get; set; }
		public IEnumerable<TEntity> Data { get; set; }
	}
}
