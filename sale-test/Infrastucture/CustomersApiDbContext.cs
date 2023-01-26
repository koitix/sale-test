using Microsoft.EntityFrameworkCore;
using sale_test.Models.Customers;
using sale_test.Models.Orders;

namespace sale_test.Infrastucture
{
    public class CustomersApiDbContext :DbContext
    {
        public CustomersApiDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
