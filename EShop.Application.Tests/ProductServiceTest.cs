using EShop.Application.Services;
using EShop.Domain.Models;
using EShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

//popraw tu test, bo usesqlserver ci nadpisuje i dlatego inmemory nie działa, więc lekka kraksa

namespace EShop.Application.Tests;

public class ProductServiceTests
{
    private DataContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new DataContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public void AddProduct_ShouldAddProduct()
    {
        // Arrange
        var context = GetInMemoryContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        var category = new Category { name = "TestCategory" };
        var product = new Product
        {
            name = "Test Product",
            ean = "123456789",
            price = 99.99m,
            stock = 10,
            sku = "TEST-001",
            category = category,
            created_by = Guid.NewGuid(),
            updated_by = Guid.NewGuid()
        };

        // Act
        service.Add(product);
        var all = service.GetAll();

        // Assert
        Assert.Single(all);
        Assert.Equal("Test Product", all.First().name);
    }

    [Fact]
    public void GetById_ShouldReturnCorrectProduct()
    {
        var context = GetInMemoryContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        var product = new Product
        {
            name = "Test",
            ean = "123",
            price = 10,
            stock = 1,
            sku = "SKU-123",
            category = new Category { name = "Books" },
            created_by = Guid.NewGuid(),
            updated_by = Guid.NewGuid()
        };

        service.Add(product);
        var result = service.GetById(product.id);

        Assert.NotNull(result);
        Assert.Equal("Test", result!.name);
    }

    [Fact]
    public void UpdateProduct_ShouldChangeProductValues()
    {
        var context = GetInMemoryContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        var product = new Product
        {
            name = "Original",
            ean = "321",
            price = 10,
            stock = 5,
            sku = "SKU-321",
            category = new Category { name = "Electronics" },
            created_by = Guid.NewGuid(),
            updated_by = Guid.NewGuid()
        };

        service.Add(product);

        product.name = "Updated";
        service.Update(product);

        var updated = service.GetById(product.id);
        Assert.Equal("Updated", updated!.name);
    }

    [Fact]
    public void DeleteProduct_ShouldRemoveProduct()
    {
        var context = GetInMemoryContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        var product = new Product
        {
            name = "ToDelete",
            ean = "000",
            price = 5,
            stock = 3,
            sku = "SKU-DEL",
            category = new Category { name = "Fashion" },
            created_by = Guid.NewGuid(),
            updated_by = Guid.NewGuid()
        };

        service.Add(product);
        service.Delete(product.id);

        var result = service.GetById(product.id);
        Assert.Null(result);
    }

    [Fact]
    public void GetByCategory_ShouldReturnFilteredProducts()
    {
        var context = GetInMemoryContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        var category = new Category { name = "Books" };

        service.Add(new Product
        {
            name = "Book 1",
            ean = "111",
            price = 15,
            stock = 20,
            sku = "BOOK-001",
            category = category,
            created_by = Guid.NewGuid(),
            updated_by = Guid.NewGuid()
        });

        service.Add(new Product
        {
            name = "Other",
            ean = "222",
            price = 10,
            stock = 5,
            sku = "ELEC-001",
            category = new Category { name = "Electronics" },
            created_by = Guid.NewGuid(),
            updated_by = Guid.NewGuid()
        });

        var books = service.GetByCategory("Books").ToList();
        Assert.Single(books);
        Assert.Equal("Book 1", books[0].name);
    }
}
