using DataLayer.Interfaces;

namespace DataLayer.Tests
{
    public class ProductRepositoryTests
    {
        private readonly MockProductRepository _mockProductRepository;
        private readonly IProductRepository _productRepository;

        public ProductRepositoryTests()
        {
            _mockProductRepository = new MockProductRepository();
            _productRepository = _mockProductRepository;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllProducts()
        {
            // Act
            var result = await _productRepository.GetAllAsync();

            // Assert
            Assert.Equal(15, result.Count());  // Assuming your mock repository has 15 products
            Assert.Contains(result, p => p.Name == "Mock Product 1");
            Assert.Contains(result, p => p.Name == "Mock Product 2");
        }

        [Fact]
        public async Task GetPaginatedAsync_ReturnsPaginatedProducts()
        {
            // Act
            var resultPage1 = await _productRepository.GetPaginatedAsync(1, 5); // Page 1 with 5 products
            var resultPage2 = await _productRepository.GetPaginatedAsync(2, 5); // Page 2 with 5 products

            // Assert
            Assert.Equal(5, resultPage1.Count()); // Verify correct number of products on page 1
            Assert.Equal(5, resultPage2.Count()); // Verify correct number of products on page 2
            Assert.Contains(resultPage1, p => p.Name == "Mock Product 1");
            Assert.Contains(resultPage2, p => p.Name == "Mock Product 6");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectProduct()
        {
            // Act
            var result = await _productRepository.GetByIdAsync(1); // Fetch product with ID 1

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Mock Product 1", result?.Name);  // Check for correct product name
        }

        [Fact]
        public async Task UpdateDescriptionAsync_UpdatesProductDescription_ReturnsTrue()
        {
            // Arrange
            var productId = 1;
            var newDescription = "Updated Mock description";

            // Act
            var result = await _productRepository.UpdateDescriptionAsync(productId, newDescription);
            var updatedProduct = await _productRepository.GetByIdAsync(productId);

            // Assert
            Assert.True(result);
            Assert.Equal(newDescription, updatedProduct?.Description); // Ensure description updated
        }

        [Fact]
        public async Task UpdateDescriptionAsync_WhenProductDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var nonExistingProductId = 99; // Non-existing product ID
            var newDescription = "Non-existent product description";

            // Act
            var result = await _productRepository.UpdateDescriptionAsync(nonExistingProductId, newDescription);

            // Assert
            Assert.False(result); // It should return false as product does not exist
        }
    }
}
