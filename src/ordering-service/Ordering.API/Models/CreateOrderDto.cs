namespace LoadLogic.Services.Ordering.API.Models
{
    public class CreateOrderDto
    {
        public int OrderNo { get; set; }
        public long ContractorId { get; set; }
    }
}
