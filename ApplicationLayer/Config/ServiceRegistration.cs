using ApplicationLayer.Mapping;
using DataLayer.Config;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ApplicationLayer.Config
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(
            this IServiceCollection services, string? connectionString, bool useMockRepository = false)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));

            if (!string.IsNullOrEmpty(connectionString))
            {
                services.AddDataLayer(connectionString, useMockRepository);
            }

            return services;
        }
    }
}
