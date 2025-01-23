using Azure.Core;
using DataLayer.Helper;
using DataLayer.Interfaces;
using DataLayer.ModelsDbo;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductModelDbo>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Products.AsNoTracking().ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<ProductModelDbo>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {           
            return await _context.Products
                .Skip((pageNumber - 1) * pageSize)   
                .Take(pageSize)                      
                .ToListAsync(cancellationToken);                      
        }
        public async Task<ProductModelDbo> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<bool> UpdateDescriptionAsync(int id, string? description)
        {
            var rowsAffected = await _context.Products
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(p => p.SetProperty(product => product.Description, description));

            return rowsAffected > 0;
        }
    }
}
