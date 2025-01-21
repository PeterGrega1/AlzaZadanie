using ApplicationLayer.ModelsDto;
using AutoMapper;
using DataLayer.Interfaces;
using DataLayer.ModelsDbo;
using MediatR;


namespace ApplicationLayer.Queries
{
    public record GetProductsQueryV1 : IRequest<IEnumerable<ProductModelDto>>;

    public class GetProductsQueryHandlerV1 : IRequestHandler<GetProductsQueryV1, IEnumerable<ProductModelDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper; 
        public GetProductsQueryHandlerV1(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper; 
        }

        public async Task<IEnumerable<ProductModelDto>> Handle(GetProductsQueryV1 request, CancellationToken cancellationToken)
        {
            IEnumerable<ProductModelDbo> products = await _productRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<ProductModelDto>>(products);
        }
    }
}

