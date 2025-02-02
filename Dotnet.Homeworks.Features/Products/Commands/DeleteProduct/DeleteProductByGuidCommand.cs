using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;

public class DeleteProductByGuidCommand : IOrderOwnerRequest, ICommand
{
    public DeleteProductByGuidCommand(Guid guid)
    {
        Guid = guid;
    }
    
    public Guid Guid { get; init; }
    public Guid OrderId { get; set; }
}