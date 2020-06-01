using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsUnitTest
    {
        [Fact]
        public async Task GetAllProducts()
        {

            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetAllProducts)).Options;
            var dbContext = new ProductsDbContext(options);
            SeedData(dbContext);


            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var products = await productsProvider.GetProductsAsync();

            Assert.True(products.IsSuccess);
            Assert.NotNull(products.Products);
            Assert.Null(products.ErrorMessage);

        }

        [Fact]
        public async Task GetAllProductWithValidId()
        {

            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetAllProducts)).Options;
            var dbContext = new ProductsDbContext(options);
            
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var product = await productsProvider.GetProductAsync(1);

            Assert.True(product.IsSuccess);
            Assert.NotNull(product.Product);
            Assert.Null(product.ErrorMessage);
        }


        [Fact]
        public async Task GetAllProductWithInvalidId()
        {

            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetAllProducts)).Options;
            var dbContext = new ProductsDbContext(options);
         

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var product = await productsProvider.GetProductAsync(-1);

            Assert.True(!product.IsSuccess);
            Assert.Null(product.Product);
            Assert.NotNull(product.ErrorMessage);
        }

        private void SeedData(ProductsDbContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                for (var i = 1000; i < 1025; i++)
                {
                    dbContext.Add(new Db.Product()
                    {
                        Id = i,
                        Name = Guid.NewGuid().ToString(),
                        Inventory = i * 5,
                        Price = (double)i * 45
                    });
                }
                dbContext.SaveChanges();
            }
            

        }
    }
}
