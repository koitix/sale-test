using sale_test.Models.Orders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace sale_test.Models.Customers
{
    public class Customer
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Cpf { get; set; }
        public int Age { get; set; }
        [AllowNull]
        public string Email { get; set; }
        [NotMapped]
        public List<Order>? OrderList { get; set; }

    }
}
