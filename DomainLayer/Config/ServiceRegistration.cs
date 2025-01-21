using DataLayer.Helper;
using DataLayer.Interfaces;
using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Config
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddDataLayer(
            this IServiceCollection services, string? connectionString, bool useMockRepository = false)
        {
            if (useMockRepository)
            {
                // Use Mock Repository
                services.AddSingleton<IProductRepository, MockProductRepository>();
            }
            else
            {
                // Use Real Database Repository
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));

                services.AddScoped<IProductRepository, ProductRepository>();
            }

            return services;
        }
    }
}
