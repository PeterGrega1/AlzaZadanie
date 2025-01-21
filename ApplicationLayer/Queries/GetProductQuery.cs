using ApplicationLayer.ModelsDto;
using AutoMapper;
using DataLayer.Interfaces;
using DataLayer.ModelsDbo;
using MediatR;


namespace ApplicationLayer.Queries
{
    public record GetProductQuery(int Id) : IRequest<ProductModelDto>;

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductModelDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper; 
        public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductModelDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            ProductModelDbo product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                return null;
            }

            return _mapper.Map<ProductModelDto>(product);
        }
    }
}

