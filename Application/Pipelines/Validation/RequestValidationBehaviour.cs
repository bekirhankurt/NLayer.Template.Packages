using FluentValidation;
using MediatR;

namespace Application.Pipelines.Validation;

public class RequestValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }


    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<object>(request);
        var failures = _validators.Select(validator => validator.Validate(context)).SelectMany(result => result.Errors)
            .Where(failure => failure != null).ToList();
        if (failures.Count != 0) throw new ValidationException(failures);
        return next();

    }
}