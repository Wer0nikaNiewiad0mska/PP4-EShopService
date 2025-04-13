using Moq;
using Xunit;
using EShop.Application.Interfaces;
using EShopService.Controllers;
using Microsoft.AspNetCore.Mvc;
using EShop.Domain.Models;

namespace EShopService.Tests.Controllers;

public class ProductControllerTest
{
    private readonly Mock<IProductService> _mockService;
    private readonly ProductController _controller;

    public ProductControllerTest()
    {
        _mockService = new Mock<IProductService>();
        _controller = new ProductController(_mockService.Object);
    }
    [Fact]
    public void Get_ShouldReturnAllProducts_ReturnTrue()
    {
        var products = new List<Product> { new Product(), new Product() };
        _mockService.Setup(s => s.GetAll()).Returns(products);
        var result = _controller.Get();
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(products, okResult.Value);
    }

    [Fact]
    public void Get_WithValidId_ReturnsProduct_ReturnTrue()
    {
        var product = new Product { id = 1 };
        _mockService.Setup(s => s.GetById(1)).Returns(product);
        var result = _controller.Get(1);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(product, okResult.Value);
    }

    [Fact]
    public void Get_WithInvalidId_ReturnsNotFound()
    {
        _mockService.Setup(s => s.GetById(It.IsAny<int>())).Returns((Product)null);
        var result = _controller.Get(999);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Post_ValidProduct_ReturnsCreatedProduct()
    {
        var newProduct = new Product();

        var result = _controller.Post(newProduct);

        _mockService.Verify(s => s.Add(newProduct), Times.Once);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(newProduct, createdAtActionResult.Value);
    }

    [Fact]
    public void Put_ValidProduct_UpdatesAndReturnsNoContent()
    {
        var product = new Product { id = 1 };
        _mockService.Setup(s => s.GetById(1)).Returns(product);

        var result = _controller.Put(1, product);

        _mockService.Verify(s => s.Update(product), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_ValidId_ReturnsNoContent()
    {
        var product = new Product { id = 1, deleted = false };
        _mockService.Setup(s => s.GetById(1)).Returns(product);

        var result = _controller.Delete(1);

        _mockService.Verify(s => s.Delete(1), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }
}
