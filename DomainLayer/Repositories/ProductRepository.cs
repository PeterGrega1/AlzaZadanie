using DataLayer.Interfaces;
using DataLayer.ModelsDbo;

namespace DataLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public async Task<IEnumerable<ProductModelDbo>> GetAllAsync()
        {
            return null;
        }
    }
}
