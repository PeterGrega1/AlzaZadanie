using ApplicationLayer.ModelsDto;
using ApplicationLayer.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PresentationLayer.Controllers;

namespace PresentationLayer.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<ILogger<ProductController>> _mockLogger;
        private readonly Mock<IMediator> _mockMediator;
        private readonly ProductController _controller;
        private readonly MockProductRepository _mockRepository;

        public ProductControllerTests()
        {
            _mockLogger = new Mock<ILogger<ProductController>>();
            _mockMediator = new Mock<IMediator>();
            _mockRepository = new MockProductRepository();
            _controller = new ProductController(_mockLogger.Object, _mockMediator.Object);
        }

        [Fact]
        public async Task GetV1_ReturnsOkResultWithProducts()
        {
            // Arrange
            var products = (await _mockRepository.GetAllAsync()).Select(p => new ProductModelDto
            {
                Id = p.Id,
                Name = p.Name,
                ImgUri = p.ImgUri,
                Price = p.Price,
                Description = p.Description
            });

            _mockMediator.Setup(m => m.Send(It.IsAny<GetProductsQueryV1>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(products);

            // Act
            var result = await _controller.GetV1();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(products, okResult.Value);
        }

        [Fact]
        public async Task GetV2_ReturnsOkResultWithPaginatedProducts()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var products = (await _mockRepository.GetPaginatedAsync(pageNumber, pageSize)).Select(p => new ProductModelDto
            {
                Id = p.Id,
                Name = p.Name,
                ImgUri = p.ImgUri,
                Price = p.Price,
                Description = p.Description
            });

            _mockMediator.Setup(m => m.Send(It.Is<GetProductsQueryV2>(q => q.PageNumber == pageNumber && q.PageSize == pageSize), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(products);

            // Act
            var result = await _controller.GetV2(pageNumber, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(products, okResult.Value); // Works because records compare by value.
        }


        [Fact]
        public async Task GetProductById_ReturnsNotFoundWhenProductDoesNotExist()
        {
            // Arrange
            var productId = 99; // An ID outside the mock data range
            _mockMediator.Setup(m => m.Send(It.Is<GetProductQuery>(q => q.Id == productId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((ProductModelDto)null);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Product with ID {productId} was not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetProductById_ReturnsOkResultWithProduct()
        {
            // Arrange
            var productId = 1;
            var product = await _mockRepository.GetByIdAsync(productId);
            var productDto = new ProductModelDto
            {
                Id = product.Id,
                Name = product.Name,
                ImgUri = product.ImgUri,
                Price = product.Price,
                Description = product.Description
            };

            _mockMediator.Setup(m => m.Send(It.Is<GetProductQuery>(q => q.Id == productId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(productDto);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(productDto, okResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsBadRequestForInvalidId()
        {
            // Arrange
            var invalidDto = new UpdateModelDto { Id = 0, Description = "Invalid" };

            // Act
            var result = await _controller.UpdateProduct(invalidDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid product ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOkResultWithSuccessStatus()
        {
            // Arrange
            var updateDto = new UpdateModelDto { Id = 1, Description = "Updated Description" };
            var updateSuccess = await _mockRepository.UpdateDescriptionAsync(updateDto.Id, updateDto.Description);

            _mockMediator.Setup(m => m.Send(It.Is<UpdateProductQuery>(q => q.data == updateDto), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(updateSuccess);

            // Act
            var result = await _controller.UpdateProduct(updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updateSuccess, okResult.Value);
        }
    }
}
