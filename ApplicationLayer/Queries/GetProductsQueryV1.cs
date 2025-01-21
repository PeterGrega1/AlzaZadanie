using ApplicationLayer.ModelsDto;
using DataLayer.Interfaces;
using MediatR;


namespace ApplicationLayer.Queries
{
    public record GetProductsQueryV2(int PageNumber = 1, int PageSize = 10) : IRequest<IEnumerable<ProductModelDto>>;

    public class GetProductsQueryHandlerV2 : IRequestHandler<GetProductsQueryV2, IEnumerable<ProductModelDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandlerV2(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductModelDto>> Handle(GetProductsQueryV2 request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();

            var paginatedProducts = products
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            return paginatedProducts.Select(product => new ProductModelDto
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

