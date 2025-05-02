using DomainLayer.Exceptions;
using Microsoft.AspNetCore.Http;
using Shared.ErrorModels;
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
				await HandleNotFoundEndPointAsync(httpContext);
			}
			catch (Exception ex)
			{
				//Log the Exception in the Error Log History of the Server (By Default, log in the Console)
				_logger.LogError(ex, "Something Went Wrong");
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private static async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
		{
			//Response Object:
			var response = new ErrorToReturn()
			{
				ErrorMessage = ex.Message
			};

			response.StatusCode = ex switch
			{
				NotFoundException => StatusCodes.Status404NotFound,
				UnauthorizedException => StatusCodes.Status401Unauthorized,
				BadRequestException badRequestException => GetBadRequestErrors(badRequestException, response),
				_ => StatusCodes.Status500InternalServerError
			};//In the Body of the Response


			//Set the Status Code of the Response:
			//httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			//OR
			httpContext.Response.StatusCode = response.StatusCode; //In the Response Context

			////Set the Content Type of the Response:
			//httpContext.Response.ContentType = "application/json";

			//Return the Response Object as JSON:
			await httpContext.Response.WriteAsJsonAsync(response); //Will Serilize the object to JSON and return it
																   //Will Automatically Set the Content Type with application/json
		}

		private static int GetBadRequestErrors(BadRequestException badRequestException, ErrorToReturn response)
		{
			response.Errors = badRequestException.Errors;
			return StatusCodes.Status400BadRequest;
		}

		private static async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
		{
			if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
			{
				var response = new ErrorToReturn()
				{
					StatusCode = StatusCodes.Status404NotFound,
					ErrorMessage = $"End Point {httpContext.Request.Path} Is Not Found"
				};
				await httpContext.Response.WriteAsJsonAsync(response);
			}
		}
	}
}
