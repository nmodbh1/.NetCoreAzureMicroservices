using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class ProductsService : IProductService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ProductsService> logger;

        public ProductsService(IHttpClientFactory httpClientFactory, ILogger<ProductsService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }
        public async Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                // get the http client
                var client = httpClientFactory.CreateClient("ProductsService");
                
                var response = await client.GetAsync(client.BaseAddress + "api/products");
                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var products = JsonSerializer.Deserialize<IEnumerable<Product>>(results, options);
                    return (true, products, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex) {
                logger.LogError(ex.Message.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
