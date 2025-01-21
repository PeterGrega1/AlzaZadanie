using ApplicationLayer.ModelsDto; 
using DataLayer.Interfaces;
using DataLayer.ModelsDbo;
using MediatR;
using AutoMapper; 

namespace ApplicationLayer.Queries
{
    public record GetProductsQueryV2(int PageNumber = 1, int PageSize = 10) : IRequest<IEnumerable<ProductModelDto>>;

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
            IEnumerable<ProductModelDbo> paginatedProducts = await _productRepository.GetPaginatedAsync(request.PageNumber, request.PageSize);

            return _mapper.Map<IEnumerable<ProductModelDto>>(paginatedProducts);
        }
    }
}
