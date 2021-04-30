using System.Collections.Generic;

namespace LoadLogic.Services.Ordering.Application.Models.Orders
{
    public class CreateRouteDto
    {
        public IEnumerable<CreateRouteLegDto> RouteLegs { get; set; } = new List<CreateRouteLegDto>();
    }
}
