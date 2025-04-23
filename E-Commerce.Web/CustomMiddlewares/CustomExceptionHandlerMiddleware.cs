using E_Commerce.Web.ErrorModels;
using System.Net;

namespace E_Commerce.Web.CustomMiddlewares
{
	public class CustomExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _nextMiddleware;
		private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

		public CustomExceptionHandlerMiddleware(RequestDelegate nextMiddleware, ILogger<CustomExceptionHandlerMiddleware> logger)
		{
			_nextMiddleware = nextMiddleware;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _nextMiddleware.Invoke(httpContext);
			}
			catch (Exception ex)
			{
				//Log the Exception in the Error Log History of the Server (By Default, log in the Console)
				_logger.LogError(ex, "Something Went Wrong");

				//Set the Status Code of the Response:
				//httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				//OR
				httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;  //In the Response

				////Set the Content Type of the Response:
				//httpContext.Response.ContentType = "application/json";

				//Response Object:
				var response = new ErrorToReturn()
				{
					StatusCode = StatusCodes.Status500InternalServerError,  //In the Body of the Response
					ErrorMessage = ex.Message
				};

				//Return the Response Object as JSON:
				await httpContext.Response.WriteAsJsonAsync(response); //Will Serilize the object to JSON and return it
																	        //Will Automatically Set the Content Type with application/json
			}
		}
	}
}
