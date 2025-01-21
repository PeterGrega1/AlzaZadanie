using ApplicationLayer.ModelsDto;
using DataLayer.Interfaces;
using MediatR;


namespace ApplicationLayer.Queries
{
    public record UpdateProductQuery(UpdateModelDto data) : IRequest<bool>;

    public class UpdateProductQueryHandler : IRequestHandler<UpdateProductQuery, bool>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductQuery request, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}

