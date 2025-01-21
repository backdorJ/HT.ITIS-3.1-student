using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.UserManagement.Commands.DeleteUserByAdmin;

public class DeleteUserByAdminCommandHandler : ICommandHandler<DeleteUserByAdminCommand> 
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserByAdminCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteUserByAdminCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Guid);
        
        var userRepo = _unitOfWork.UserRepository;

        try
        {
            // var isExist = await _unitOfWork.UserRepository.GetUserByGuidAsync(request.Guid, cancellationToken);
            //
            // if (isExist == null)
            //     return new Result(false, $"User {request.Guid} does not exist.");
            
            await userRepo.DeleteUserByGuidAsync(request.Guid, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return new Result(true);
        }
        catch (Exception e)
        {
            return new Result(false, e.Message);
        }
    }
}