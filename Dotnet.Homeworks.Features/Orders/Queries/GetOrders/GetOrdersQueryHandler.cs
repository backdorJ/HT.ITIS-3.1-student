using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, GetOrdersDto>
{
    private readonly IOrderRepository _orderRepository;
    
    private readonly IHttpContextAccessor _context;

    public GetOrdersQueryHandler(IOrderRepository orderRepository, IHttpContextAccessor context)
    {
        _orderRepository = orderRepository;
        _context = context;
    }
    
    public async Task<Result<GetOrdersDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        try
        {
            var userId = _context.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid parsedUserId))
            {
                return new Result<GetOrdersDto>(null, false, "Invalid user Id");
            }
            
            var orders = (await _orderRepository
                .GetAllOrdersFromUserAsync(parsedUserId, cancellationToken))
                .Select(x => new GetOrderDto(x.Id, x.ProductsIds))
                .ToList();

            return new Result<GetOrdersDto>(new GetOrdersDto(orders), true);
        }
        catch (Exception e)
        {
            return new Result<GetOrdersDto>(null, false, e.Message);
        }
    }
}