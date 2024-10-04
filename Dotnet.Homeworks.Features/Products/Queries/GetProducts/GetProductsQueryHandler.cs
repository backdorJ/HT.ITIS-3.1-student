using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Queries.GetProducts;

internal sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetProductsDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var productsFromDb = await _unitOfWork.ProductRepository.GetAllProductsAsync(cancellationToken);

        var result = productsFromDb
            .Select(x => new GetProductDto(x.Id, x.Name))
            .ToList();

        return new Result<GetProductsDto>(new GetProductsDto(result), true);
    }
}