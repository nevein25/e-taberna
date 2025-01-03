using FluentValidation;
using Humanizer;
using MediatR;

namespace ShoppingCart.API.Behaviors;

// instead of injecting the validator in each handler class
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>

{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) 
                                    //request: incoming from client, next:  representing the next request handle delegate, the next pipeline behavior or actual handle method
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults =
            await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures =
            validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
            throw new ValidationException(failures);

        return await next(); // This will run the next pipeline behavior, which will be our actual command handle method.  This is very crucial to call this next method to continue this pipeline request lifecycle.


    }
}