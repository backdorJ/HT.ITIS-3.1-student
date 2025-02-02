using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    private readonly IProductRepository _productRepository;
    
    public CreateOrderValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;
        
        RuleForEach(x => x.ProductsIds)
            .MustAsync(IsExistsProductsAsync)
            .WithMessage("Product not found");
        
        RuleFor(x => x.ProductsIds)
            .Must(x => x.Any())
            .WithMessage("Products not found");
    }

    private async Task<bool> IsExistsProductsAsync(Guid product, CancellationToken cancellationToken)
    {
        return await _productRepository.GetProductByIdAsync(product, cancellationToken) != null;
    }
}