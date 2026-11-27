using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Application.Requests.ProjectRequests;
using TimeSheet.Application.Project.Commands.CreateProject;
using TimeSheet.Application.Project.Commands.DeleteProject;
using TimeSheet.Application.Project.Commands.UpdateProject;
using TimeSheet.Application.Project.Queries.GetProjectById;
using TimeSheet.Application.Project.Queries.GetProjects;
using TimeSheet.Application.Requests.ProjectResponses;
using TimeSheet.Api.Authorization;

namespace TimeSheet.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/project")]
public class ProjectController : ControllerBase
{
    private readonly IMediator Mediator;

    public ProjectController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ProjectResponse>> GetAll([FromQuery]bool includeInactive, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetProjectsQuery(includeInactive), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetProjectByIdQuery(id), cancellationToken));


    }

    [HttpPost]
    [Authorize(Roles =Roles.Admin)]
    public async Task<ActionResult<ProjectResponse>> Create(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateProjectCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);

    }
    [HttpDelete("{id:guid}")]
    [Authorize(Roles =Roles.Admin)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteProjectCommand(id), cancellationToken);
        return NoContent();

    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles =Roles.Admin)]
    public async Task<ActionResult> Update(Guid id, UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UpdateProjectCommand(id, request), cancellationToken);
        return NoContent();

    }

}