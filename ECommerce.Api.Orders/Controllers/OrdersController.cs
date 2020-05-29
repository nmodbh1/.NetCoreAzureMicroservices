using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersProvider ordersProvider;

        public OrdersController(IOrdersProvider ordersProvider)
        {
            this.ordersProvider = ordersProvider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdersAsync(int id)
        {
            var results = await ordersProvider.GetOrdersAsync(id);
            if (results.IsSuccess)
            {
                return Ok(results.Orders);
            }
            return NotFound();
        }

    }
}
