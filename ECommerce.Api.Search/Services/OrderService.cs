using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientFactory httpClient;

        public OrderService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {

                var client = httpClient.CreateClient("OrderService");

                var response = await client.GetAsync(client.BaseAddress + "api/orders/"  + $"{customerId}");

                if (response.IsSuccessStatusCode)
                {
                    var results = response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;

                    var orders = JsonSerializer.Deserialize<IEnumerable<Order>>(results.Result, options);
                    return (true, orders, null);
                }
                return (false, null, "Not Found");
            }
            catch(Exception ex)
            {

                return (false, null, ex.Message);
            }
        }
    }
}
