using MediatR;
using Microsoft.Extensions.Logging;


namespace ApplicationLayer.Config
{
    public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

        public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                return await next(); // Continue the pipeline
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("Request was canceled: {RequestName}", typeof(TRequest).Name);
                return await next(); // Continue the pipeline. Query handler need cancel sql command

            }
            catch (Exception ex)
            {
                // Handle and log other exceptions
                _logger.LogError(ex, "An unexpected error occurred while processing: {RequestName}", typeof(TRequest).Name);
                throw; // Re-throw other exceptions
            }
        }
    }



}
