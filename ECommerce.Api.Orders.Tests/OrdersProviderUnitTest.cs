using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Profiles;
using ECommerce.Api.Orders.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Orders.Tests
{
    public class OrdersProviderUnitTest
    {
        [Fact]
        public async Task GetAllOrdersWdValidId()
        {

            var options = new DbContextOptionsBuilder<OrdersDbContext>()
                .UseInMemoryDatabase(nameof(GetAllOrdersWdValidId))
                .Options;
            var dbContext = new OrdersDbContext(options);
            SeedData(dbContext);

            var ordersProfile = new OrderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(ordersProfile));
            var mapper = new Mapper(configuration);


            var ordersProvider = new OrdersProvider(dbContext, null, mapper);
            var results = await ordersProvider.GetOrdersAsync(1);
            Assert.True(results.IsSuccess);
            Assert.NotNull(results.Orders);
            Assert.Null(results.ErrorMessage);
        }

        [Fact]
        public async Task GetAllOrdersWdInvalidId()
        {

            var options = new DbContextOptionsBuilder<OrdersDbContext>()
                .UseInMemoryDatabase(nameof(GetAllOrdersWdValidId))
                .Options;
            var dbContext = new OrdersDbContext(options);
            SeedData(dbContext);

            var ordersProfile = new OrderProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(ordersProfile));
            var mapper = new Mapper(configuration);


            var ordersProvider = new OrdersProvider(dbContext, null, mapper);
            var results = await ordersProvider.GetOrdersAsync(-1);
            
            Assert.False(results.IsSuccess);
            Assert.Null(results.Orders);
            Assert.NotNull(results.ErrorMessage);
        }
        private void SeedData(OrdersDbContext dbContext)
        {
            if (!dbContext.Orders.Any())
            {
                for (var i = 100; i <= 110; i++)
                {
                    dbContext.Add(new Db.Order()
                    {
                        Id = i,
                        CustomerId = i + 1,
                        OrderDate = DateTime.Now,
                        Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem() { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 10 },
                        new Db.OrderItem() { OrderId = 2, ProductId = 2, Quantity = 10, UnitPrice = 10 },
                        new Db.OrderItem() { OrderId = 3, ProductId = 3, Quantity = 1, UnitPrice = 100 }
                    },
                        Total = i * 100
                    }); ;
                }

            }
        }
    }
}
