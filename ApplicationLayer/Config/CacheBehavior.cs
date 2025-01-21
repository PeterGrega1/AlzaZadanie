using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Config
{
    public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheBehavior<TRequest, TResponse>> _logger;

        public CacheBehavior(IMemoryCache cache, ILogger<CacheBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var cacheKey = request.GetHashCode().ToString();

            if (_cache.TryGetValue(cacheKey, out TResponse response))
            {
                _logger.LogInformation($"Returning cached response for {typeof(TRequest).Name}");
                return response;
            }

            _logger.LogInformation($"Cache miss for {typeof(TRequest).Name}. Invoking handler.");
            response = await next();

            _cache.Set(cacheKey, response, TimeSpan.FromMinutes(5));

            return response;
        }
    }
}
