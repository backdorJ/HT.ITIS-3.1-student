using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Dto;
using Dotnet.Homeworks.Infrastructure.Services;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : 
    CqrsDecorator<CreateUserCommand, Result<CreateUserDto>>,
    ICommandHandler<CreateUserCommand, CreateUserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRegistrationService _registrationService;

    public CreateUserCommandHandler(
        IEnumerable<IValidator<CreateUserCommand>> validators,
        IPermissionCheck permissionCheck,
        IUnitOfWork unitOfWork,
        IRegistrationService registrationService
    ) : base(validators, permissionCheck)
    {
        _unitOfWork = unitOfWork;
        _registrationService = registrationService;
    }

    public async Task<Result<CreateUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
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
            
            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
            };
            
            var id = await userRepo.InsertUserAsync(user, cancellationToken);
            
            await _registrationService.RegisterAsync(new RegisterUserDto(request.Name, request.Email), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return new Result<CreateUserDto>(new CreateUserDto(id), true);
        }
        catch (Exception e)
        {
            return new Result<CreateUserDto>(null, false, e.Message); 
        }
    }
}