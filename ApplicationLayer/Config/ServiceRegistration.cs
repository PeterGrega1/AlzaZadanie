using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ApplicationLayer.Config
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // Register MediatR and application-specific services
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            // Add application-specific pipeline behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));

            return services;
        }
    }
}
