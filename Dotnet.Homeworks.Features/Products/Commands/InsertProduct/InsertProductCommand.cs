using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

namespace Dotnet.Homeworks.Features.Products.Commands.InsertProduct;

public class InsertProductCommand : ICommand<InsertProductDto> 
{
    public InsertProductCommand(string name)
    {
        Name = name;
    }
    
    public string Name { get; init; }
}
