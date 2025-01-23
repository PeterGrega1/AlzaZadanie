using DataLayer.ModelsDbo;

namespace DataLayer.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModelDbo>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<ProductModelDbo>> GetPaginatedAsync(int pageNumber, int pageSize,CancellationToken cancellationToken);
        Task<ProductModelDbo> GetByIdAsync(int id);
        Task<bool> UpdateDescriptionAsync(int id, string? description);
    }
}
