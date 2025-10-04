using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.Web.Factories
{
	public static class ApiResponseFactory
	{
		public static IActionResult GenerateApiValidationErrorsResponse(ActionContext context)
		{
			var errors = context.ModelState.Where(M => M.Value.Errors.Any())
												   .Select(modelError => new ValidationError()
												   {
													   Field = modelError.Key,
													   Errors = modelError.Value.Errors.Select(error => error.ErrorMessage)
												   });
			var response = new ValidationErrorToReturn()
			{
				ValidationErrors = errors
			};
			return new BadRequestObjectResult(response);
		}
	}
}
