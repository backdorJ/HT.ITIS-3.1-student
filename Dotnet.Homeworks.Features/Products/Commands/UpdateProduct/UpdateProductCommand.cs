using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<Result>
{
    public UpdateProductCommand(Guid guid, string name)
    {
        Guid = guid;
        Name = name;
    }
    
    public Guid Guid { get; init; }
    public string Name { get; init; }
}