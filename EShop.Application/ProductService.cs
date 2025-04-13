using EShop.Application.Interfaces;
using EShop.Domain.Models;
using EShop.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IEnumerable<Product> GetAll() => _productRepository.GetAll();

    public Product? GetById(int id) => _productRepository.GetById(id); 

    public void Add(Product product) => _productRepository.Add(product);

    public void Update(Product product) => _productRepository.Update(product);

    public void Delete(int id) => _productRepository.Delete(id);

    public IEnumerable<Product> GetByCategory(string categoryName)
        => _productRepository.GetByCategory(categoryName);

    public bool Exists(int id) => _productRepository.Exists(id);

    public int Count() => _productRepository.Count();
}