using ApplicationLayer.Queries;
using ApplicationLayer.ModelsDto;
using DataLayer.Interfaces;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationLayer.Tests
{
    public class UpdateProductQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly UpdateProductQueryHandler _handler;

        public UpdateProductQueryHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new UpdateProductQueryHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_ValidProduct_ReturnsTrue()
        {
            // Arrange
            var updateModelDto = new UpdateModelDto
            {
                Id = 1,
                Description = "Updated description"
            };

            _mockProductRepository.Setup(repo => repo.UpdateDescriptionAsync(updateModelDto.Id, updateModelDto.Description))
                                  .ReturnsAsync(true); // Simulating a successful update

            var query = new UpdateProductQuery(updateModelDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockProductRepository.Verify(repo => repo.UpdateDescriptionAsync(updateModelDto.Id, updateModelDto.Description), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidProduct_ReturnsFalse()
        {
            // Arrange
            var updateModelDto = new UpdateModelDto
            {
                Id = 99,  // Assuming this product doesn't exist in the repository
                Description = "Updated description"
            };

            _mockProductRepository.Setup(repo => repo.UpdateDescriptionAsync(updateModelDto.Id, updateModelDto.Description))
                                  .ReturnsAsync(false); // Simulating an unsuccessful update when product doesn't exist

            var query = new UpdateProductQuery(updateModelDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mockProductRepository.Verify(repo => repo.UpdateDescriptionAsync(updateModelDto.Id, updateModelDto.Description), Times.Once);
        }
    }
}
