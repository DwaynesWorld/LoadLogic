namespace LoadLogic.Services.Ordering.API.Controllers.V1.Models
{
    public class CreateOrderLineItemApiRequest
    {
        public CreateRouteApiRequest? Route { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public string MaterialUnit { get; set; } = string.Empty;
        public double MaterialQuantity { get; set; }
        public double MaterialWeight { get; set; }
        public string MaterialDimensions { get; set; } = string.Empty;
    }
}
