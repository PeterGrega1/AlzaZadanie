using ApplicationLayer.ModelsDto;
using ApplicationLayer.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("GetProducts")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetV1()
    {
        _logger.LogInformation("Fetching all products...");
        return Ok(await _mediator.Send(new GetProductsQueryV1(), HttpContext.RequestAborted));
    }


    [HttpGet("GetProducts")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetV2(int? pageNumber, int? pageSize)
    {
        _logger.LogInformation("Fetching paginated products.");
        return Ok(await _mediator.Send(new GetProductsQueryV2(pageNumber, pageSize), HttpContext.RequestAborted));
    }

    [HttpGet($"{nameof(GetProductById)}/{{id}}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        _logger.LogInformation("Fetching product by ID.");
        return  Ok(await _mediator.Send(new GetProductQuery(id)));
    }

    [HttpPut(nameof(UpdateProduct))]
    public async Task<IActionResult> UpdateProduct(UpdateModelDto updateDescrition)
    {
        _logger.LogInformation("Updating product.");
        return Ok(await _mediator.Send(new UpdateProductQuery(updateDescrition)));
    }
}
