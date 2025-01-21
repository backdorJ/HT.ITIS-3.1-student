using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.InsertProduct;

internal sealed class InsertProductCommandHandler : IRequestHandler<InsertProductCommand, Result<InsertProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public InsertProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<InsertProductDto>> Handle(
        InsertProductCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var productRepo = _unitOfWork.ProductRepository;

        try
        {
            var guid = await productRepo.InsertProductAsync(new Product
                {
                    Name = request.Name
                },
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new Result<InsertProductDto>(new InsertProductDto(guid), true);
        }
        catch (Exception e)
        {
            return new Result<InsertProductDto>(null, false, e.Message);
        }
    }
}