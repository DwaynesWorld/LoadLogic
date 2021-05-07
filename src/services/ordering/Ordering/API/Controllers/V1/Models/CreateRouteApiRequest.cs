using System.Collections.Generic;

namespace LoadLogic.Services.Ordering.API.Controllers.V1.Models
{
    public class CreateRouteApiRequest
    {
        public IEnumerable<CreateRouteLegApiRequest> RouteLegs { get; set; } = new List<CreateRouteLegApiRequest>();
    }
}
