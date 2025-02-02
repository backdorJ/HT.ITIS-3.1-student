using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var productRepo = _unitOfWork.ProductRepository;

        try
        {
            var product = await productRepo.GetProductByIdAsync(request.Guid, cancellationToken);

            if (product is null)
                return new Result(false, $"Не найден товар с идентификатором {request.Guid}");

            product.Name = request.Name;
            
            await productRepo.UpdateProductAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new Result(true);
        }
        catch (Exception e)
        {
            return new Result(false, e.Message);
        }
    }
}