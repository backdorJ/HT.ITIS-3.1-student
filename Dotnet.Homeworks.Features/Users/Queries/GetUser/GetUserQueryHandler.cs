using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public class GetUserQueryHandler :     
    CqrsDecorator<GetUserQuery, Result<GetUserDto>>,
    IQueryHandler<GetUserQuery, GetUserDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserQueryHandler(
        IEnumerable<IValidator<GetUserQuery>> validators,
        IPermissionCheck permissionCheck,
        IUnitOfWork unitOfWork
    ) : base(validators, permissionCheck)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetUserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
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
            
            return user == null
                ? new Result<GetUserDto>(null, false, "User not found")
                : new Result<GetUserDto>(new GetUserDto(user.Id, user.Name, user.Email), true);
        }
        catch (Exception e)
        {
            return new Result<GetUserDto>(null, false, e.Message); 
        }
    }
}