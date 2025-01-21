using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, GetAllUsersDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllUsersDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var userRepo = _unitOfWork.UserRepository;
            
            var users = (await userRepo.GetUsersAsync(cancellationToken))
                .Select(x => new GetUserDto(x.Id, x.Name, x.Email))
                .ToList();
            
            return new Result<GetAllUsersDto>(new GetAllUsersDto(users), true);
        }
        catch (Exception e)
        {
            return new Result<GetAllUsersDto>(null,false, e.Message);
        }
    }
}