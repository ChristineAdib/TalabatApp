using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace TalabatAPIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSecond;

        public CachedAttribute(int ExpireTimeInSecond)
        {
            _expireTimeInSecond = ExpireTimeInSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var CachedKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var CachedResponse = await CachedService.GetCacheResponse(CachedKey);

            if (!string.IsNullOrEmpty(CachedResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = CachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result= contentResult;
                return;
            }

            var ExecutedEndPointContext = await next.Invoke();
            if(ExecutedEndPointContext.Result is OkObjectResult result)
            {
                await CachedService.CacheResponseAsync(CachedKey, result.Value, TimeSpan.FromSeconds(_expireTimeInSecond));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path);
            foreach(var (key, value) in request.Query.OrderBy(x=>x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
