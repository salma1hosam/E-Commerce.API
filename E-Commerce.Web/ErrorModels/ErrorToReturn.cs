namespace E_Commerce.Web.ErrorModels
{
	public class ErrorToReturn
	{
		public int StatusCode { get; set; }
		public string ErrorMessage { get; set; } = default!;
	}
}
