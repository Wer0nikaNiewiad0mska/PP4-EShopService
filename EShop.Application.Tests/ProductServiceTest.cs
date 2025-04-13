using EShop.Application.Services;
using EShop.Domain.Models;
using EShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


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

        var category = new Category { Name = "TestCategory" };
        var product = new Product
        {
            Name = "Test Product",
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
        Assert.Equal("Test Product", all.First().Name);
    }

    [Fact]
    public void GetById_ShouldReturnCorrectProduct()
    {
        var context = GetInMemoryContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        var product = new Product
        {
            Name = "Test",
            ean = "123",
            price = 10,
            stock = 1,
            sku = "SKU-123",
            category = new Category { Name = "Books" },
            created_by = Guid.NewGuid(),
            updated_by = Guid.NewGuid()
        };

        service.Add(product);
        var result = service.GetById(product.id);

        Assert.NotNull(result);
        Assert.Equal("Test", result!.Name);
    }

    [Fact]
    public void UpdateProduct_ShouldChangeProductValues()
    {
        var context = GetInMemoryContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        var product = new Product
        {
            Name = "Original",
            ean = "321",
            price = 10,
            stock = 5,
            sku = "SKU-321",
            category = new Category { Name = "Electronics" },
            created_by = Guid.NewGuid(),
            updated_by = Guid.NewGuid()
        };

        service.Add(product);

        product.Name = "Updated";
        service.Update(product);

        var updated = service.GetById(product.id);
        Assert.Equal("Updated", updated!.Name);
    }

    [Fact]
    public void DeleteProduct_ShouldRemoveProduct()
    {
        var context = GetInMemoryContext();
        var repo = new ProductRepository(context);
        var service = new ProductService(repo);

        var product = new Product
        {
            Name = "ToDelete",
            ean = "000",
            price = 5,
            stock = 3,
            sku = "SKU-DEL",
            category = new Category { Name = "Fashion" },
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

        var category = new Category { Name = "Books" };

        service.Add(new Product
        {
            Name = "Book 1",
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
            Name = "Other",
            ean = "222",
            price = 10,
            stock = 5,
            sku = "ELEC-001",
            category = new Category { Name = "Electronics" },
            created_by = Guid.NewGuid(),
            updated_by = Guid.NewGuid()
        });

        var books = service.GetByCategory("Books").ToList();
        Assert.Single(books);
        Assert.Equal("Book 1", books[0].Name);
    }
}
