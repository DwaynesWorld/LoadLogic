using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backups.Application.Behaviors
{
    /// <summary>
    /// The logging behavior for general logging of MediatR requests.
    /// </summary>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType().Name;
            var requestNameWithGuid = $"[{requestName}] [{Guid.NewGuid()}]";

            TResponse response;

            try
            {
                _logger.LogInformation("[START] {requestNameWithGuid} {@request}", requestNameWithGuid, request);
                response = await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ERROR] {requestNameWithGuid}", requestNameWithGuid);
                throw;
            }

            _logger.LogInformation("[END] {requestNameWithGuid} {@response}", requestNameWithGuid, response);
            return response;
        }
    }
}
