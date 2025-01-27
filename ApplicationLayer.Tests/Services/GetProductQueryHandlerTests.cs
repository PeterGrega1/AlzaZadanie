using ApplicationLayer.Queries;
using ApplicationLayer.ModelsDto;
using AutoMapper;
using DataLayer.Interfaces;
using DataLayer.ModelsDbo;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationLayer.Tests
{
    public class GetProductQueryHandlerTests
    {
        private readonly MockProductRepository _mockProductRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetProductQueryHandler _handler;

        public GetProductQueryHandlerTests()
        {
            _mockProductRepository = new MockProductRepository();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetProductQueryHandler(_mockProductRepository, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ProductExists_ReturnsProductDto()
        {
            // Arrange
            int productId = 1;
            var productDbo = new ProductModelDbo
            {
                Id = productId,
                Name = "Mock Product 1",
                ImgUri = "http://example.com/product_1.jpg",
                Price = 11.99m,
                Description = "Mock description for Product 1"
            };

            var productDto = new ProductModelDto
            {
                Id = productId,
                Name = "Mock Product 1",
                ImgUri = "http://example.com/product_1.jpg",
                Price = 11.99m,
                Description = "Mock description for Product 1"
            };

            _mockMapper.Setup(m => m.Map<ProductModelDto>(It.IsAny<ProductModelDbo>()))
                .Returns(productDto);

            var query = new GetProductQuery(productId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result?.Id);
            Assert.Equal("Mock Product 1", result?.Name);
            Assert.Equal(11.99m, result?.Price);
        }

        [Fact]
        public async Task Handle_ProductDoesNotExist_ReturnsNull()
        {
            // Arrange
            int productId = 999; // ID not present in the repository
            var query = new GetProductQuery(productId);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.Equal($"Product with ID {productId} was not found.", exception.Message);
        }


    }
}
