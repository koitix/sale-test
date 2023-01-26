using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sale_test.Infrastucture;
using sale_test.Models.Customers;
using System.Linq;
using System.Xml.Linq;

namespace sale_test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly CustomersApiDbContext dbContext;
        public CustomerController(CustomersApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            return Ok(await dbContext.Customers.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] Guid id)
        {

            var customer = await dbContext.Customers.FindAsync(id);

            if (customer != null)
            {

                return Ok(customer);
            }

            return NotFound("No Customer founded.");
        }


        [HttpGet]
        [Route("{nameSearch}")]
        public async Task<IActionResult> GetCustomerByName([FromRoute] String nameSearch)
        {
            if (nameSearch.Trim().Length > 0)
            {
                var l_customer = await dbContext.Customers.ToListAsync();

                l_customer = l_customer.FindAll(x => x.Name.ToLower().Contains(nameSearch.ToLower()));


                if (l_customer != null)
                {

                    return Ok(l_customer);
                }
            }

            return NotFound("No Customer founded.");
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(AddCustomerRequest request)
        {


            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Cpf = request.Cpf,
                BirthDate = request.BirthDate,
                Age = AgeCalculator(request.BirthDate),
                Email = request.Email,

            };

            await dbContext.Customers.AddAsync(customer);
            await dbContext.SaveChangesAsync();

            return Ok(customer);
        }

        [HttpPost]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCostumer([FromRoute] Guid id, UpdateCustomerRequest request)
        {

            var customer = await dbContext.Customers.FindAsync(id);

            if (customer != null)
            {
                customer.Name = request.Name;
                customer.Cpf = request.Cpf;
                customer.BirthDate = request.BirthDate;
                customer.Age = AgeCalculator(request.BirthDate);
                customer.Email = request.Email;
                await dbContext.SaveChangesAsync();
                return Ok(customer);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
        {

            var customer = await dbContext.Customers.FindAsync(id);

            if (customer != null)
            {
                //remover os orders
                customer.OrderList = (List<Models.Orders.Order>?)dbContext.Orders.ToList().Where(x => x.CustomerId == customer.Id.ToString());

                if (customer.OrderList != null)
                    foreach (var item in customer.OrderList)
                    {
                        dbContext.Orders.Remove(item);
                    }

                dbContext.Remove(customer);
                await dbContext.SaveChangesAsync();
                return Ok("Customer deleted");
            }

            return Ok("Not Found");
        }


        private int AgeCalculator(DateTime birthDate)
        {

            int calculatedAge = DateTime.Today.Year - birthDate.Year;

            if (birthDate.Month > DateTime.Today.Month || birthDate.Month == DateTime.Today.Month && birthDate.Day > DateTime.Today.Day)
                calculatedAge--;

            if (calculatedAge < 0)
            {
                calculatedAge = 0;
            }

            return calculatedAge;
        }
    }
}
