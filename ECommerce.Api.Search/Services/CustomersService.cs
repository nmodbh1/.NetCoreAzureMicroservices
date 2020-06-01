using ECommerce.Api.Search.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class CustomersService : ICustomersServices
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<CustomersService> logger;

        public CustomersService(IHttpClientFactory httpClientFactory, ILogger<CustomersService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<(bool IsSuccess, dynamic Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var client = httpClientFactory.CreateClient("CustomersService");

                var response = await client.GetAsync("api/customers/" + $"{id}");

                if (response.IsSuccessStatusCode)
                {
                    var contents = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var customer = JsonSerializer.Deserialize<dynamic>(contents, options);
                    return (true, customer, "");
                }
                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex){
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
