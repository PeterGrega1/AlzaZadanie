using ApplicationLayer.ModelsDto;
using ApplicationLayer.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // Version 1 endpoint (using MediatR)
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetV1()
        {
            try
            {
                _logger.LogInformation("Fetching all products (v1).");
                var products = await _mediator.Send(new GetProductsQueryV1()); // Send query using MediatR
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products (v1).");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Version 2 endpoint with pagination (using MediatR)
        [HttpGet]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetV2(int pageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation("Fetching paginated products (v2).");
                var products = await _mediator.Send(new GetProductsQueryV2(pageNumber, pageSize));
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products (v2).");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Uncomment and fix this endpoint if you want to fetch a product by ID (version 2 example)
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetProductById(int id)
        //{
        //    try
        //    {
        //        _logger.LogInformation("Fetching product by ID (v2).");
        //        var query = new GetProductQuery(id);
        //        var product = await _mediator.Send(query); // Send the query using MediatR
        //        return Ok(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error fetching product by ID (v2).");
        //        return StatusCode(500, "An unexpected error occurred.");
        //    }
        //}
    }
}
