
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Application.Category;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Responses.CategoryResponses;

namespace TimeSheet.Api.Controllers;


[ApiController]
[Route("api/categories")]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator=mediator;
    }

    [HttpGet]
    public async Task<ActionResult>GetAll()
    {

        return Ok(await _mediator.Send(new GetCategoriesQuery()));

    }
}