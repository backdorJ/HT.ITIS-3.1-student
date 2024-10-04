using Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Mvc;
using IMediator = MediatR.IMediator;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class ProductManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductsQuery(), cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("product")]
    public async Task<IActionResult> InsertProduct(
        [FromBody] string name,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new InsertProductCommand(name), cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("product")]
    public async Task<IActionResult> DeleteProduct(Guid guid, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteProductByGuidCommand(guid), cancellationToken);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok();
    }

    [HttpPut("product")]
    public async Task<IActionResult> UpdateProduct(Guid guid, string name, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateProductCommand(guid, name), cancellationToken);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok();
    }
}