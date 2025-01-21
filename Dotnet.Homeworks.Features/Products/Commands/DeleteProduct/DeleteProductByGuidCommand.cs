using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;

public class DeleteProductByGuidCommand : IRequest<Result>
{
    public DeleteProductByGuidCommand(Guid guid)
    {
        Guid = guid;
    }
    
    public Guid Guid { get; init; }
}