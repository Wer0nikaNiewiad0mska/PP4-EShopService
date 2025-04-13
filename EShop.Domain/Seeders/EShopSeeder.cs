using EShop.Domain.Models;
using EShop.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Seeders
{
    public class EShopSeeder(DataContext context) : IEShopSeeder
    {
        public async Task Seed()
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Electronics", created_by = Guid.NewGuid(), updated_by = Guid.NewGuid() },
                    new Category { Name = "Books", created_by = Guid.NewGuid(), updated_by = Guid.NewGuid() },
                    new Category { Name = "Fashion", created_by = Guid.NewGuid(), updated_by = Guid.NewGuid() }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                var electronics = context.Categories.FirstOrDefault(c => c.Name == "Electronics");
                var books = context.Categories.FirstOrDefault(c => c.Name == "Books");
                var fashion = context.Categories.FirstOrDefault(c => c.Name == "Fashion");

                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "Smartphone XYZ",
                        ean = "1234567890123",
                        price = 1999.99m,
                        stock = 15,
                        sku = "ELEC-XYZ-001",
                        category = electronics!,
                        created_by = Guid.NewGuid(),
                        updated_by = Guid.NewGuid()
                    },
                    new Product
                    {
                        Name = "„C# Programming” Book",
                        ean = "9876543210987",
                        price = 79.90m,
                        stock = 50,
                        sku = "BOOK-CSHARP-001",
                        category = books!,
                        created_by = Guid.NewGuid(),
                        updated_by = Guid.NewGuid()
                    },
                    new Product
                    {
                        Name = "Casual T-Shirt",
                        ean = "5556667778881",
                        price = 49.99m,
                        stock = 120,
                        sku = "FASH-TSHIRT-001",
                        category = fashion!,
                        created_by = Guid.NewGuid(),
                        updated_by = Guid.NewGuid()
                    }
                };

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}