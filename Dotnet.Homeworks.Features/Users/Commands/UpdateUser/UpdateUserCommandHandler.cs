using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler :  CqrsDecorator<UpdateUserCommand, Result>, ICommandHandler<UpdateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(
        IEnumerable<IValidator<UpdateUserCommand>> validators,
        IPermissionCheck permissionCheck,
        IUnitOfWork unitOfWork
    ) : base(validators, permissionCheck)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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
            
            var user = await userRepo.GetUserByGuidAsync(request.Guid, cancellationToken);
            
            if (user == null)
                return new Result(false, "User not found");
            
            user.Email = request.User.Email;
            user.Name = request.User.Name;
            
            await userRepo.UpdateUserAsync(user, cancellationToken);
            
            return new Result(true);
        }
        catch (Exception e)
        {
            return new Result(false, e.Message);
        }
    }
}