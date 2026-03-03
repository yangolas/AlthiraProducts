using AlthiraProducts.BuildingBlocks.Aplication.Pagination;
using AlthiraProducts.Products.Application.Commands;
using AlthiraProducts.Products.Application.Dtos.RequestsApi;
using AlthiraProducts.Products.Application.Dtos.ResponsesApi;
using AlthiraProducts.Products.Application.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlthiraProducts.Adapters.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : Controller
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator) => _mediator = mediator;

    [HttpGet()]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetProducts([FromQuery] PagedRequest paginationRequest)
    {
        PagedResult<ProductDto> pagedResult = await _mediator.Send(new GetProductsQuery(paginationRequest));
        return Ok(pagedResult);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateProduct([FromForm] CreateProductDto createProductDto)
    {
        Guid sku = await _mediator.Send(new CreateProductCommand(createProductDto));
        return CreatedAtAction(nameof(GetProducts), new { sku }, new { sku });
    }
}