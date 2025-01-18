using Order.SharedKernel.CQRS;
using Order.SharedKernel.Results;

namespace Order.SharedKernel.Messaging;
public interface ISender
{
    Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);

}
