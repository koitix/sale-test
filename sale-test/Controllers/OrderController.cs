using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sale_test.Infrastucture;
using sale_test.Models.Orders;

namespace sale_test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly CustomersApiDbContext dbContext;
        public OrderController(CustomersApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await dbContext.Orders.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id)
        {

            var order = await dbContext.Orders.FindAsync(id);

            if (order != null)
            {

                return Ok(order);
            }

            return NotFound("No Order founded.");
        }


        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderRequest request)
        {


            var order = new Order()
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                Price = request.Price,
                PaidAt = request.PaidAt,
                status = request.status

            };

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            return Ok(order);
        }

        [HttpPost]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] Guid id, UpdateOrderRequest request)
        {

            var order = await dbContext.Orders.FindAsync(id);

            if (order != null)
            {

                order.CustomerId = request.CustomerId;
                order.Price = request.Price;
                order.PaidAt = request.PaidAt;
                order.status = request.status;
                await dbContext.SaveChangesAsync();
                return Ok(order);
            }

            return NotFound();
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id)
        {

            var order = await dbContext.Orders.FindAsync(id);

            if (order != null)
            {                
                dbContext.Remove(order);
                await dbContext.SaveChangesAsync();
                return Ok("Order deleted");
            }

            return Ok("Not Found");
        }

    }
}
