using DataLayer.Helper;
using DataLayer.Interfaces;
using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Config
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, string connectionString)
        {
            // Register DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register repositories
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
