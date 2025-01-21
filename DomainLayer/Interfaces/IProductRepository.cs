using DataLayer.ModelsDbo;

namespace DataLayer.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModelDbo>> GetAllAsync();
    }
}
