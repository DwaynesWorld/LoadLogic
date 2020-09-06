using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace LoadLogic.Services.Vendors.API.Models.Examples
{
    public class ProblemDetailsExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            var example = new ProblemDetails();
            example.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
            example.Title = "Not Found";
            example.Detail = "Entity MinorityType with key 0 was not found.";
            example.Instance = "api/minoritytypes/0";
            example.Status = StatusCodes.Status400BadRequest;
            return example;
        }
    }
}
