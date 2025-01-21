using ApplicationLayer.ModelsDto;
using ApplicationLayer.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMediator _mediator;

        public ProductController(ILogger<ProductController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // Version 1 endpoint 
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetV1()
        {
            try
            {
                _logger.LogInformation("Fetching all products.");
                var products = await _mediator.Send(new GetProductsQueryV1());
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Version 2 endpoint with pagination 
        [HttpGet]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetV2(int pageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation("Fetching paginated products.");
                var products = await _mediator.Send(new GetProductsQueryV2(pageNumber, pageSize));
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching product by ID.");
                var product = await _mediator.Send(new GetProductQuery(id));

                return product == null ? NotFound($"Product with ID {id} was not found.") : Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product by ID.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("{data}")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateModelDto updateDescrition)
        {
            try
            {
                if (updateDescrition.Id <= 0)
                {
                    return BadRequest("Invalid product ID.");
                }
                _logger.LogInformation("Udpating product");

                var product = await _mediator.Send(new UpdateProductQuery(updateDescrition));
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product by ID.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
