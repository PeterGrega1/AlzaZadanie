using DataLayer.Interfaces;
using DataLayer.ModelsDbo;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Helper
{
    public static class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider, bool useMockData)
        {
            if (useMockData)
            {
                serviceProvider.GetRequiredService<IProductRepository>();
                return;
            }

            using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            if (context.Products.Any()) return;

            context.Products.AddRange(
                new ProductModelDbo { Name = "Product A", ImgUri = "http://example.com/images/product_a.jpg", Price = 10.99m, Description = "Description for Product A" },
                new ProductModelDbo { Name = "Product B", ImgUri = "http://example.com/images/product_b.jpg", Price = 20.49m, Description = "Description for Product B" },
                new ProductModelDbo { Name = "Product C", ImgUri = "http://example.com/images/product_c.jpg", Price = 15.79m, Description = "Description for Product C" },
                new ProductModelDbo { Name = "Product D", ImgUri = "http://example.com/images/product_d.jpg", Price = 33.99m, Description = "Description for Product D" },
                new ProductModelDbo { Name = "Product E", ImgUri = "http://example.com/images/product_e.jpg", Price = 45.49m, Description = "Description for Product E" },
                new ProductModelDbo { Name = "Product F", ImgUri = "http://example.com/images/product_f.jpg", Price = 12.99m, Description = "Description for Product F" },
                new ProductModelDbo { Name = "Product G", ImgUri = "http://example.com/images/product_g.jpg", Price = 8.79m, Description = "Description for Product G" },
                new ProductModelDbo { Name = "Product H", ImgUri = "http://example.com/images/product_h.jpg", Price = 18.69m, Description = "Description for Product H" },
                new ProductModelDbo { Name = "Product I", ImgUri = "http://example.com/images/product_i.jpg", Price = 23.39m, Description = "Description for Product I" },
                new ProductModelDbo { Name = "Product J", ImgUri = "http://example.com/images/product_j.jpg", Price = 29.99m, Description = "Description for Product J" },
                new ProductModelDbo { Name = "Product K", ImgUri = "http://example.com/images/product_k.jpg", Price = 7.49m, Description = "Description for Product K" },
                new ProductModelDbo { Name = "Product L", ImgUri = "http://example.com/images/product_l.jpg", Price = 11.99m, Description = "Description for Product L" },
                new ProductModelDbo { Name = "Product M", ImgUri = "http://example.com/images/product_m.jpg", Price = 26.59m, Description = "Description for Product M" },
                new ProductModelDbo { Name = "Product N", ImgUri = "http://example.com/images/product_n.jpg", Price = 37.19m, Description = "Description for Product N" },
                new ProductModelDbo { Name = "Product O", ImgUri = "http://example.com/images/product_o.jpg", Price = 40.99m, Description = "Description for Product O" }
            );

            context.SaveChanges();
        }
    }
}
