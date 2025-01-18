
namespace Order.SharedKernel.CQRS;
public interface ICommand : IBaseCommand
{
}


public interface ICommand<TResponse> : IBaseCommand
{
}

public interface IBaseCommand
{
}
