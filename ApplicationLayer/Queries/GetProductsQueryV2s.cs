using ApplicationLayer.ModelsDto; 
using DataLayer.Interfaces;
using DataLayer.ModelsDbo;
using MediatR;
using AutoMapper; 

namespace ApplicationLayer.Queries
{
    public record GetProductsQueryV2(int? PageNumber, int? PageSize) : IRequest<IEnumerable<ProductModelDto>>;

    public class GetProductsQueryHandlerV2 : IRequestHandler<GetProductsQueryV2, IEnumerable<ProductModelDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper; 

        public GetProductsQueryHandlerV2(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductModelDto>> Handle(GetProductsQueryV2 request, CancellationToken cancellationToken)
        {
            IEnumerable<ProductModelDbo> paginatedProducts = await _productRepository.GetPaginatedAsync(request.PageNumber ?? 1, request.PageSize ?? 10);

            return _mapper.Map<IEnumerable<ProductModelDto>>(paginatedProducts);
        }
    }
}
