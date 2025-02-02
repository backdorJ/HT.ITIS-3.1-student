using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using MongoDB.Driver;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _ordersCollection;

    public OrderRepository(IMongoDatabase database)
    {
        _ordersCollection = database.GetCollection<Order>("Orders");
    }

    public async Task<IEnumerable<Order>> GetAllOrdersFromUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.OrdererId, userId);
        var result = await _ordersCollection
            .Find(filter)
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<Order?> GetOrderByGuidAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.Id, orderId);
        return await _ordersCollection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteOrderByGuidAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.Id, orderId);
        var deleteResult = await _ordersCollection
            .DeleteOneAsync(filter, cancellationToken);

        if (deleteResult.DeletedCount == 0)
        {
            throw new ArgumentNullException(
                nameof(orderId),
                $"Order with Id = {orderId} was not found for deletion."
            );
        }
    }

    public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.Id, order.Id);
        await _ordersCollection
            .ReplaceOneAsync(filter, order, cancellationToken: cancellationToken);
    }

    public async Task<Guid> InsertOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await _ordersCollection
            .InsertOneAsync(order, cancellationToken: cancellationToken);

        return order.Id;
    }
}