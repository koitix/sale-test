namespace sale_test.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public decimal Price { get; set; }
        public bool status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
