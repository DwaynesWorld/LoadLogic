
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LoadLogic.Services.Ordering.API
{
    public class SwaggerOperationIdOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.OperationId = context.MethodInfo.Name;
        }
    }
}
