using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IUserRepository _userRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IHttpContextAccessor httpContext, IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _httpContext = httpContext;
        _userRepository = userRepository;
    }

    public async Task<Result<CreateOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        
        var userId = _httpContext.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid parsedUserId))
        {
            return new Result<CreateOrderDto>(null, false, "Invalid user Id");
        }

        var currentUser = await _userRepository.GetUserByGuidAsync(parsedUserId, cancellationToken);
        
        if (currentUser == null)
            return new Result<CreateOrderDto>(null, false, "User not found");

        var order = new Order()
        {
            OrdererId = parsedUserId,
            ProductsIds = request.ProductsIds,
        };

        try
        {
            var guid = await _orderRepository.InsertOrderAsync(order, cancellationToken);

            return new Result<CreateOrderDto>(new CreateOrderDto(guid), true);
        }
        catch (Exception e)
        {
            return new Result<CreateOrderDto>(null, false, e.Message);
        }
    }
}