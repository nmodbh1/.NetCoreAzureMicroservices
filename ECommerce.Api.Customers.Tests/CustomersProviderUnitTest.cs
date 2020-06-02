using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Providers;
using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using ECommerce.Api.Customers.Profiles;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Tests
{
    public class CustomersProviderUnitTest
    {
        [Fact]
        public async Task GetAllCustomersReturnsAllCustomers()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>()
                .UseInMemoryDatabase(nameof(GetAllCustomersReturnsAllCustomers))
                .Options;
            var dbContext = new CustomersDbContext(options);
            SeedData(dbContext);

            var customerProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customerProfile));
            var mapper = new Mapper(configuration);

            var customersProvider = new CustomersProvider(dbContext, null, mapper);
            var results = await customersProvider.GetCustomersAsync();

            Assert.True(results.IsSuccess);
            Assert.NotNull(results.Customers);
            Assert.Null(results.ErrorMessage);
        }

        private void SeedData(CustomersDbContext dbContext)
        {
            if (!dbContext.Customers.Any())
            {
                for(int i=100; i < 110; i++){

                    dbContext.Add(new Db.Customer()
                    {
                        Id = i,
                        Name = $"John Dow{i}",
                        Address = $"Salt Lake, Street {i}, Hathway, US"
                    });

                }
            }
        }

        [Fact]
        public async Task GetCustomerWithValidId()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>()
                .UseInMemoryDatabase(nameof(GetAllCustomersReturnsAllCustomers))
                .Options;
            var dbContext = new CustomersDbContext(options);
            SeedData(dbContext);

            var customerProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customerProfile));
            var mapper = new Mapper(configuration);

            var customersProvider = new CustomersProvider(dbContext, null, mapper);
            var results = await customersProvider.GetCustomerAsync(100);

            Assert.True(results.IsSuccess);
            Assert.NotNull(results.Customer);
            Assert.Null(results.ErrorMessage);
        }

        [Fact]
        public async Task GetCustomerWithInvalidId()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>()
                .UseInMemoryDatabase(nameof(GetAllCustomersReturnsAllCustomers))
                .Options;
            var dbContext = new CustomersDbContext(options);
            SeedData(dbContext);

            var customerProfile = new CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customerProfile));
            var mapper = new Mapper(configuration);

            var customersProvider = new CustomersProvider(dbContext, null, mapper);
            var results = await customersProvider.GetCustomerAsync(-1);

            Assert.False(results.IsSuccess);
            Assert.Null(results.Customer);
            Assert.NotNull(results.ErrorMessage);
        }
    }
}
