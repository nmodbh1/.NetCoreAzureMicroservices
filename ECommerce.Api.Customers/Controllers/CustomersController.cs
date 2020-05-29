using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Providers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerProvider customersProvider;

        public CustomersController(ICustomerProvider customersProvider)
        {
            this.customersProvider = customersProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var customers = await customersProvider.GetCustomersAsync();
            if(customers.IsSuccess){
                return Ok(customers.Customers);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var results = await customersProvider.GetCustomerAsync(id);
            if (results.IsSuccess)
            {
                return Ok(results.Customer);
            }
            return NotFound();
        }
    }
}
