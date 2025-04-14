using EShop.Domain.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using EShop.Domain.Models;

namespace EShopService.Integration.Tests.Controllers;

public class ProductControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public ProductControllerIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services
                    .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataContext>));

                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("MyDBForTest"));
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsAllProducts()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            dbContext.Products.RemoveRange(dbContext.Products);
            dbContext.Categories.RemoveRange(dbContext.Categories);
            await dbContext.SaveChangesAsync();

            var category = new Category
            {
                Name = "TestCategory",
                created_by = Guid.NewGuid(),
                updated_by = Guid.NewGuid()
            };
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();

            dbContext.Products.AddRange(
                new Product
                {
                    Name = "Product1",
                    ean = "1111111111111",
                    sku = "SKU-001",
                    price = 10.0m,
                    stock = 100,
                    category = category,
                    created_by = Guid.NewGuid(),
                    updated_by = Guid.NewGuid()
                },
                new Product
                {
                    Name = "Product2",
                    ean = "2222222222222",
                    sku = "SKU-002",
                    price = 20.0m,
                    stock = 50,
                    category = category,
                    created_by = Guid.NewGuid(),
                    updated_by = Guid.NewGuid()
                }
            );
            await dbContext.SaveChangesAsync();
        }

        var response = await _client.GetAsync("/api/product");

        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.Equal(2, products?.Count);
    }
}