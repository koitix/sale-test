using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sale_test.Infrastucture;
using sale_test.Models.Customers;
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

            var order_list = await dbContext.Orders.ToListAsync();
            order_list = order_list.FindAll(x => x.CustomerId == request.CustomerId);
            if (order_list.Any())
            {
                foreach (var item in order_list)
                {
                    if (item.status == false)
                    {
                        return BadRequest("There is some open Order");
                    }
                }
            }

            var order = new Order()
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                Price = request.Price,
                PaidAt = request.PaidAt,
                status = request.status,
                CreatedAt = DateTime.Now

            };

            if (order.status)
            {
                order.PaidAt = DateTime.Now;
            }

            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            return Ok(order);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] Guid id, UpdateOrderRequest request)
        {

            var order = await dbContext.Orders.FindAsync(id);

            if (order != null)
            {

                order.CustomerId = request.CustomerId;
                order.Price = request.Price;
                if (order.status)
                {
                    order.PaidAt = DateTime.Now;
                }
                else
                {
                    order.PaidAt = request.PaidAt;
                }
                order.status = request.status;
                await dbContext.SaveChangesAsync();
                return Ok(order);
            }

            return NotFound();
        }

        [HttpPut]
        [Route("UpdatePayOrder/{id:guid}")]
        public async Task<IActionResult> UpdatePayOrder([FromRoute] Guid id)
        {

            var order = await dbContext.Orders.FindAsync(id);

            if (order != null)
            {
                order.PaidAt = DateTime.Now;
                order.status = true;
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
                return Ok(order);
            }

            return Ok("Not Found");
        }

    }
}
