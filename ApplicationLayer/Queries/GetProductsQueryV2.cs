using ApplicationLayer.ModelsDto;
using DataLayer.Interfaces;
using MediatR;


namespace ApplicationLayer.Queries
{
    public record GetProductsQueryV1 : IRequest<IEnumerable<ProductModelDto>>;

    public class GetProductsQueryHandlerV1 : IRequestHandler<GetProductsQueryV1, IEnumerable<ProductModelDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandlerV1(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductModelDto>> Handle(GetProductsQueryV1 request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(product => new ProductModelDto
            {
                Id = product.Id,
                Name = product.Name,
                ImgUri = product.ImgUri,
                Price = product.Price,
                Description = product.Description
            });
        }
    }
}

