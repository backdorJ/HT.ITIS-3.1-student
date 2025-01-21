using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.InsertProduct;

public class InsertProductCommand : IRequest<Result<InsertProductDto>> 
{
    public InsertProductCommand(string name)
    {
        Name = name;
    }
    
    public string Name { get; init; }
}
