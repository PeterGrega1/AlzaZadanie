using ApplicationLayer.ModelsDto;
using ApplicationLayer.Queries;
using AutoMapper;
using DataLayer.ModelsDbo;
using Moq;

namespace ApplicationLayer.Tests
{
    public class GetProductsQueryHandlerV1Tests
    {
        private readonly MockProductRepository _mockProductRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetProductsQueryHandlerV1 _handler;

        public GetProductsQueryHandlerV1Tests()
        {
            _mockProductRepository = new MockProductRepository();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetProductsQueryHandlerV1(_mockProductRepository, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ProductsExist_ReturnsProductDtos()
        {
            // Arrange
            var productsDbo = new List<ProductModelDbo>
            {
                new ProductModelDbo { Id = 1, Name = "Mock Product 1", ImgUri = "http://example.com/product_1.jpg", Price = 11.99m, Description = "Description 1" },
                new ProductModelDbo { Id = 2, Name = "Mock Product 2", ImgUri = "http://example.com/product_2.jpg", Price = 12.99m, Description = "Description 2" },
                new ProductModelDbo { Id = 3, Name = "Mock Product 3", ImgUri = "http://example.com/product_3.jpg", Price = 13.99m, Description = "Description 3" }
            };

            var productsDto = new List<ProductModelDto>
            {
                new ProductModelDto { Id = 1, Name = "Mock Product 1", ImgUri = "http://example.com/product_1.jpg", Price = 11.99m, Description = "Description 1" },
                new ProductModelDto { Id = 2, Name = "Mock Product 2", ImgUri = "http://example.com/product_2.jpg", Price = 12.99m, Description = "Description 2" },
                new ProductModelDto { Id = 3, Name = "Mock Product 3", ImgUri = "http://example.com/product_3.jpg", Price = 13.99m, Description = "Description 3" }
            };

            _mockProductRepository.SetProductList(productsDbo);  // Assuming SetProductList is a method you defined in MockProductRepository to set a list.
            _mockMapper.Setup(m => m.Map<IEnumerable<ProductModelDto>>(It.IsAny<IEnumerable<ProductModelDbo>>()))
                       .Returns(productsDto);

            var query = new GetProductsQueryV1();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result?.Count());
            Assert.Equal("Mock Product 1", result?.FirstOrDefault()?.Name);
            Assert.Equal(11.99m, result?.FirstOrDefault()?.Price);
        }

        [Fact]
        public async Task Handle_NoProducts_ReturnsEmptyList()
        {
            // Arrange
            var emptyProducts = new List<ProductModelDbo>();  // No products
            var productsDto = new List<ProductModelDto>();    // Empty DTO list

            _mockProductRepository.SetProductList(emptyProducts);  // No products in repository
            _mockMapper.Setup(m => m.Map<IEnumerable<ProductModelDto>>(It.IsAny<IEnumerable<ProductModelDbo>>()))
                       .Returns(productsDto);

            var query = new GetProductsQueryV1();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
