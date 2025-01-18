using Order.SharedKernel.CQRS;
using Order.SharedKernel.Results;

namespace Order.SharedKernel.Messaging;
public class Sender : ISender
{
    private readonly IServiceProvider _serviceProvider;

    public Sender(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"No handler found for command {command.GetType().Name}");
        }

        var method = handlerType.GetMethod("Handle") ?? throw new InvalidOperationException($"Handle method not found on {handlerType.Name}");
        var task = method.Invoke(handler, new object[] { command, cancellationToken }) as Task<Result<TResponse>>;

        return task == null
            ? throw new InvalidOperationException($"Handle method did not return the expected Task<Result<{typeof(TResponse).Name}>>")
            : await task;
    }

}
