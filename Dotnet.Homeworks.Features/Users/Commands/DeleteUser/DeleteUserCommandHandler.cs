using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : CqrsDecorator<DeleteUserCommand, Result>,
    ICommandHandler<DeleteUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(
        IEnumerable<IValidator<DeleteUserCommand>> validators,
        IPermissionCheck permissionCheck,
        IUnitOfWork unitOfWork
    ) : base(validators, permissionCheck)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var res = await base.Handle(request, cancellationToken);
        if (res.IsFailure)
        {
            return res;
        }
        
        try
        {
            var userRepo = _unitOfWork.UserRepository;

            await userRepo.DeleteUserByGuidAsync(request.Guid, cancellationToken);
            return new Result(true);
        }
        catch (Exception e)
        {
            return new Result(false, e.Message);
        }
    }
}