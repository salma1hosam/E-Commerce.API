using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;
using System.Text;

namespace Presentation.Attributes
{
	internal class CacheAttribute(int durationInSeconds = 180) : ActionFilterAttribute
	{
		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			//Create Cache Key
			var cacheKey = CreateCacheKey(context.HttpContext.Request);

			//Search for the Cache Value using the Cache Key
			ICacheService cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
			var cacheValue = await cacheService.GetAsync(cacheKey);

			//Return the Cache Value If Not Null
			if (cacheValue is not null)
			{
				context.Result = new ContentResult()
				{
					Content = cacheValue,
					ContentType = "application/json",
					StatusCode = StatusCodes.Status200OK
				};
				return;
			}

			//If Null,
			//Invoke .next (EndPoint)
			var executedContext = await next.Invoke();

			//Set the Value of the EndPoint With the Cache Key (to cahe it)
			if (executedContext.Result is OkObjectResult result)
				await cacheService.SetAsync(cacheKey, result, TimeSpan.FromSeconds(durationInSeconds));
		}

		private string CreateCacheKey(HttpRequest request)
		{
			//{{BaseUrl}}/api/Products?BrandId=4&TypeId=2

			StringBuilder key = new StringBuilder();
			key.Append(request.Path + '?');    //{{BaseUrl}}/api/Products?
			foreach (var item in request.Query.OrderBy(Q => Q.Key))
				key.Append($"{item.Key}={item.Value}&");  //BrandId=4&TypeId=2
			return key.ToString();
		}
	}
}
