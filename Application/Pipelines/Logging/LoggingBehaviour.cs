using CrossCuttingConcerns.Logging;
using CrossCuttingConcerns.Logging.Serilog;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.Pipelines.Logging;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ILoggableRequest
{
    private readonly LoggerServiceBase _loggerServiceBase;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingBehaviour(LoggerServiceBase loggerServiceBase, IHttpContextAccessor httpContextAccessor)
    {
        _loggerServiceBase = loggerServiceBase;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var logParameters = new List<LogParameter>
        {
            new LogParameter
            {
                Type = request.GetType().Name,
                Value = request
            }
        };

        var logDetail = new LogDetail
        {
            MethodName = next.Method.Name,
            Parameters = logParameters,
            User = _httpContextAccessor.HttpContext == null ||
                   _httpContextAccessor.HttpContext.User.Identity.Name == null
                ? "Not Found."
                : _httpContextAccessor.HttpContext.User.Identity.Name
        };
        
        _loggerServiceBase.Info(JsonConvert.SerializeObject(logDetail));

        return next();
    }
}