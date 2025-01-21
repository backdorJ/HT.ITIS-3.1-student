using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.UserManagement.Commands.DeleteUserByAdmin;

public class DeleteUserByAdminCommandHandler : ICommandHandler<DeleteUserByAdminCommand> 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    
    public DeleteUserByAdminCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(DeleteUserByAdminCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Guid);
        
        var userRepo = _unitOfWork.UserRepository;

        try
        {
            var isExist = await _userRepository.GetUserByGuidAsync(request.Guid, cancellationToken);
            
            if (isExist == null)
                return new Result(false, $"User {request.Guid} does not exist.");
            
            await _userRepository.DeleteUserByGuidAsync(request.Guid, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return new Result(true);
        }
        catch (Exception e)
        {
            return new Result(false, e.Message);
        }
    }
}