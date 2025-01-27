using ApplicationLayer.ModelsDto;
using ApplicationLayer.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

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
            var products = (await _mockRepository.GetAllAsync(new CancellationToken()))
                            .Select(p => new ProductModelDto
                            {
                                Id = p.Id,
                                Name = p.Name,
                                ImgUri = p.ImgUri,
                                Price = p.Price,
                                Description = p.Description
                            })
                            .ToList();

            _mockMediator.Setup(m => m.Send(It.IsAny<GetProductsQueryV1>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(products);

            // Setup HttpContext
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _controller.GetV1();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsType<List<ProductModelDto>>(okResult.Value);
            Assert.Equal(products, returnedProducts);
        }



        [Fact]
        public async Task GetV2_ReturnsOkResultWithPaginatedProducts()
        {
            // Arrange
            int? pageNumber = 1; // Set value explicitly
            int? pageSize = 10;  // Set value explicitly

            CancellationToken token = new CancellationToken();

            // Simulate repository fetch for paginated products (mock data already present)
            var products = (await _mockRepository.GetPaginatedAsync(pageNumber ?? 1, pageSize ?? 10, token)) // If null, default fallback
                            .Select(p => new ProductModelDto
                            {
                                Id = p.Id,
                                Name = p.Name,
                                ImgUri = p.ImgUri,
                                Price = p.Price,
                                Description = p.Description
                            })
                            .ToList();  // Ensure evaluation here

            // Now ensuring that mediator is set up properly with the mock
            _mockMediator.Setup(m => m.Send(It.Is<GetProductsQueryV2>(q => q.PageNumber == pageNumber && q.PageSize == pageSize), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(products)
                         .Callback(() =>
                         {
                             Console.WriteLine($"PageNumber: {pageNumber}, PageSize: {pageSize}");
                         });

            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _controller.GetV2(pageNumber, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(products, okResult.Value); // Check if products are returned correctly
        }




        [Fact]
        public async Task GetProductById_ReturnsOkResultWithNullWhenProductDoesNotExist()
        {
            // Arrange
            var productId = 99;
            _mockMediator.Setup(m => m.Send(It.Is<GetProductQuery>(q => q.Id == productId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((ProductModelDto?)null);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Null(okResult.Value); // Ensure the value of the OK response is null.
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
        public async Task UpdateProduct_ReturnsOkResultWithDefaultBehaviorForInvalidId()
        {
            // Arrange
            var invalidDto = new UpdateModelDto { Id = 0, Description = "Invalid" };

            _mockMediator.Setup(m => m.Send(It.Is<UpdateProductQuery>(q => q.data == invalidDto), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true); // Assume the operation still completes with "true".

            // Act
            var result = await _controller.UpdateProduct(invalidDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value); // Validate the result based on current behavior.
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
