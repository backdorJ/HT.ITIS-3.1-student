using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;

internal sealed class DeleteProductByGuidCommandHandler : ICommandHandler<DeleteProductByGuidCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductByGuidCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteProductByGuidCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var productRepo = _unitOfWork.ProductRepository;

        try
        {
            await productRepo.DeleteProductByGuidAsync(request.Guid, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return new Result(true);
        }
        catch (Exception e)
        {
            return new Result(false, e.Message);
        }
    }
}