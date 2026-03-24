using AlthiraProducts.BoundedContext.Products.Application.Models.Dtos;
using AlthiraProducts.BoundedContext.Products.Application.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlthiraProducts.Adapters.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        IEnumerable<CategoryDto> categories = await _mediator.Send(new GetCategoryQuery());
        return Ok(categories);
    }
}
