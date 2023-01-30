using System.Diagnostics.CodeAnalysis;

namespace sale_test.Models.Orders
{
    public class UpdateOrderRequest
    {
        public decimal Price { get; set; }
        public string CustomerId { get; set; }
        public bool status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
