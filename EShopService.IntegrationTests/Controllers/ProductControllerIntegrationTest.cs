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
    private WebApplicationFactory<Program> _factory;

    public ProductControllerIntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services
                        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataContext>));

                    services.Remove(dbContextOptions);

                    services
                        .AddDbContext<DataContext>(options => options.UseInMemoryDatabase("MyDBForTest"));

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

            dbContext.Products.AddRange(
                new Product { Name = "Product1" },
                new Product { Name = "Product2" }
            );
            await dbContext.SaveChangesAsync();
        }

        var response = await _client.GetAsync("/api/product");

        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.Equal(2, products?.Count);
    }
}
