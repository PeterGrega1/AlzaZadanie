using ApplicationLayer.ModelsDto;
using DataLayer.Interfaces;
using MediatR;


namespace ApplicationLayer.Queries
{
    public record GetProductQuery(int Id) : IRequest<ProductModelDto>;

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductModelDto>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductModelDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}

