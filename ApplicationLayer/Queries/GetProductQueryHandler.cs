using ApplicationLayer.ModelsDto;
using DataLayer.Interfaces;
using MediatR;

namespace ApplicationLayer.Queries
{
    public class GetProductQuery : IRequest<ProductModelDto>
    {
        public int Id { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductModelDto>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductModelDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            // Fetch product by Id from the repository
            //var product = await _productRepository.GetByIdAsync(request.Id);

            // Map product to ProductModelDto (adjust based on your domain object model and mapping strategy)
            return null;
        }
    }
}
