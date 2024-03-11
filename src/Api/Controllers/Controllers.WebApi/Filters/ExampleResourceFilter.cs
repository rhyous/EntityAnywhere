using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Rhyous.StringLibrary;

namespace Rhyous.EntityAnywhere.WebApi.Filters
{
    public class ExampleResourceFilter : IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (context.HttpContext.Request.Method == "POST"
                && context.HttpContext.Request.Headers.TryGetValue("Content-Type", out StringValues contentType) 
                && contentType[0] == "application/json")
            {
                var postJson = context.HttpContext.Request.Body.AsStringAsync().Result;
                // Log json here
                context.HttpContext.Request.Body = postJson.ToStream();
            }
        }
    }
}
