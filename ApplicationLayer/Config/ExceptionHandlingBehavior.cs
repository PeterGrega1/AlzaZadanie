using MediatR;
using Microsoft.Extensions.Logging;


namespace ApplicationLayer.Config
{
    public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

        public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                // Proceed to the next handler
                return await next();
            }
            catch (Exception ex)
            {
                // Log exception details
                _logger.LogError(ex, $"Error occurred while processing {typeof(TRequest).Name}");

                // Optionally, you can rethrow or wrap exceptions into domain-specific exceptions
                throw new Exception("An error occurred during processing. See inner exception for details.", ex);
            }
        }
    }
}
