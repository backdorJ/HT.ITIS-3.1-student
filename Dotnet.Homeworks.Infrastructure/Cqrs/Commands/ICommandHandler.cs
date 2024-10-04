using Dotnet.Homeworks.Shared.Dto;
using MediatR;

namespace Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : IRequest<Result>
{
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : IRequest<Result<TResponse>>
{
}