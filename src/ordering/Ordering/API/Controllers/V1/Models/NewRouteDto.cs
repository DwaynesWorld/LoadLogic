using System.Collections.Generic;

namespace LoadLogic.Services.Ordering.API.Controllers.V1.Models
{
    public class NewRouteDto
    {
        public IEnumerable<NewRouteLegDto> RouteLegs { get; set; } = new List<NewRouteLegDto>();
    }
}
