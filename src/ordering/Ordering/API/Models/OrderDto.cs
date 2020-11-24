namespace LoadLogic.Services.Ordering.API.Models
{
    public class OrderDto
    {
        public OrderDto(int orderNo, long customerId, string customerName, Email customerEmail)
        {
            this.OrderNo = orderNo;
            this.CustomerId = customerId;
            this.CustomerName = customerName;
            this.CustomerEmail = customerEmail;
        }

        public int OrderNo { get; set; }
        public long CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public Email? CustomerEmail { get; set; }
    }
}
