using DataLayer.ModelsDbo;

namespace DataLayer.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModelDbo>> GetAllAsync();
        Task<IEnumerable<ProductModelDbo>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<ProductModelDbo> GetByIdAsync(int id);
        Task<bool> UpdateDescriptionAsync(int id, string? description);
    }
}
