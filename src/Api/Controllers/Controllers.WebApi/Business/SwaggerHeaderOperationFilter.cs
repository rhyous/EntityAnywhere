using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rhyous.EntityAnywhere.WebApi
{
    internal class SwaggerHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var tokenParameter = new OpenApiParameter
            {
                Name = "Token",
                In = ParameterLocation.Header,
                Description = "The authentication token.",
                Required = false
            };

            operation.Parameters.Add(tokenParameter); 
            var adminTokenParameter = new OpenApiParameter
            {
                Name = "EntityAdminToken",
                In = ParameterLocation.Header,
                Description = "The authentication token.",
                Required = false
            };

            operation.Parameters.Add(adminTokenParameter);
        }
    }
}
