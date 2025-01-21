using DataLayer.Interfaces;
using DataLayer.ModelsDbo;
using System.Collections.Concurrent;

public class MockProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<int, ProductModelDbo> _products = new();


    public MockProductRepository()
    {
        for (int i = 1; i <= 15; i++)
        {
            _products.TryAdd(i, new ProductModelDbo
            {
                Id = i,
                Name = $"Mock Product {i}",
                ImgUri = $"http://example.com/product_{i}.jpg",
                Price = 10.99m + i,
                Description = $"Mock description for Product {i}"
            });
        }
    }
    public void SetProductList(IEnumerable<ProductModelDbo> products)
    {
        _products.Clear();  // Optionally clear existing products
        foreach (var product in products)
        {
            _products[product.Id] = product;
        }
    }

    public Task<IEnumerable<ProductModelDbo>> GetAllAsync()
    {
        return Task.FromResult(_products.Values.AsEnumerable());
    }

    public Task<IEnumerable<ProductModelDbo>> GetPaginatedAsync(int pageNumber, int pageSize)
    {
        var paginated = _products.Values
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return Task.FromResult(paginated);
    }

    public Task<ProductModelDbo> GetByIdAsync(int id)
    {
        _products.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task<bool> UpdateDescriptionAsync(int id, string? description)
    {
        if (_products.TryGetValue(id, out var product))
        {
            product.Description = description;
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}
