using ApplicationLayer.ModelsDto;
using ApplicationLayer.Queries;
using AutoMapper;
using DataLayer.Interfaces;
using DataLayer.ModelsDbo;
using Moq;

namespace ApplicationLayer.Tests
{
    public class GetProductsQueryHandlerV2Tests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetProductsQueryHandlerV2 _handler;

        public GetProductsQueryHandlerV2Tests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetProductsQueryHandlerV2(_mockProductRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ProductsExist_ReturnsPaginatedProductDtos()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 2;

            var productsDbo = new List<ProductModelDbo>
            {
                new ProductModelDbo { Id = 1, Name = "Mock Product 1", ImgUri = "http://example.com/product_1.jpg", Price = 11.99m, Description = "Description 1" },
                new ProductModelDbo { Id = 2, Name = "Mock Product 2", ImgUri = "http://example.com/product_2.jpg", Price = 12.99m, Description = "Description 2" },
                new ProductModelDbo { Id = 3, Name = "Mock Product 3", ImgUri = "http://example.com/product_3.jpg", Price = 13.99m, Description = "Description 3" }
            };

            var productsDto = new List<ProductModelDto>
            {
                new ProductModelDto { Id = 1, Name = "Mock Product 1", ImgUri = "http://example.com/product_1.jpg", Price = 11.99m, Description = "Description 1" },
                new ProductModelDto { Id = 2, Name = "Mock Product 2", ImgUri = "http://example.com/product_2.jpg", Price = 12.99m, Description = "Description 2" }
            };

            // Mock the repository and mapping
            _mockProductRepository.Setup(repo => repo.GetPaginatedAsync(pageNumber, pageSize))
                .ReturnsAsync(productsDbo.Take(pageSize));  // Simulate a paginated result
            _mockMapper.Setup(m => m.Map<IEnumerable<ProductModelDto>>(It.IsAny<IEnumerable<ProductModelDbo>>()))
                       .Returns(productsDto);

            var query = new GetProductsQueryV2(pageNumber, pageSize);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Mock Product 1", result.FirstOrDefault()?.Name);
            Assert.Equal(11.99m, result.FirstOrDefault()?.Price);
        }

        [Fact]
        public async Task Handle_NoProductsForPagination_ReturnsEmptyList()
        {
            // Arrange
            int pageNumber = 3;
            int pageSize = 5;

            var emptyProducts = new List<ProductModelDbo>();  // No products
            var productsDto = new List<ProductModelDto>();    // Empty DTO list

            _mockProductRepository.Setup(repo => repo.GetPaginatedAsync(pageNumber, pageSize))
                .ReturnsAsync(emptyProducts);  // Simulate no products available for the requested page
            _mockMapper.Setup(m => m.Map<IEnumerable<ProductModelDto>>(It.IsAny<IEnumerable<ProductModelDbo>>()))
                       .Returns(productsDto);

            var query = new GetProductsQueryV2(pageNumber, pageSize);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
