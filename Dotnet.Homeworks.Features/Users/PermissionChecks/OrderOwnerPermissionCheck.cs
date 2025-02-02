using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Users.PermissionChecks;

public class OrderOwnerPermissionCheck : IPermissionCheck<IOrderOwnerRequest>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContext;

    public OrderOwnerPermissionCheck(
        IHttpContextAccessor httpContextAccessor,
        IOrderRepository orderRepository,
        IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _httpContext = httpContextAccessor;
    }

    public async Task<PermissionResult> CheckPermission(IOrderOwnerRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContext.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid parsedUserId))
        {
            return new PermissionResult(false, "Invalid user Id");
        }

        var user = await _userRepository.GetUserByGuidAsync(parsedUserId, default);
        if (user is null)
        {
            return new PermissionResult(false, message: "User is not found");
        }

        var order = await _orderRepository.GetOrderByGuidAsync(request.OrderId, default);
        if (order is null)
        {
            return new PermissionResult(false, message: "Order is not found");
        }

        if(order.OrdererId != user.Id)
        {
            return new PermissionResult(false, message: "User does not own this order");
        }

        return new PermissionResult(true);
    }
}