using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly ICustomersServices customersServices;

        public SearchService(IOrderService orderService, IProductService productService, ICustomersServices customersServices)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.customersServices = customersServices;
        }

        public async Task<(bool IsSuccess, dynamic Results)> SearchAsync(int customerId)
        {
            var customer = await customersServices.GetCustomerAsync(customerId);
            var products = await productService.GetProductsAsync();
            var orders = await orderService.GetOrdersAsync(customerId);
            if (orders.IsSuccess)
            {
                foreach (var order in orders.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        var productName = products.IsSuccess ? products.Products.FirstOrDefault(x => x.Id == item.ProductId).Name.ToString() : "Product name is not available.";
                        item.ProductName = productName;
                    }
                }

                var results = new
                {
                    Customer = customer.IsSuccess ? customer.Customer :
                    new { Name = "Customer Information is not available." },
                    Orders = orders.Orders
                };
                return (true, results);
            }
            return (false, null);
        }
    }
}
