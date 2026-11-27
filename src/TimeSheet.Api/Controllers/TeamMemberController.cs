using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Api.Authorization;
using TimeSheet.Application.Requests.TeamMemberRequests;
using TimeSheet.Application.Responses.TeamMemberResponses;
using TimeSheet.Application.TeamMember.Commands.CreateTeamMember;
using TimeSheet.Application.TeamMember.Commands.DeleteTeamMember;
using TimeSheet.Application.TeamMember.Commands.UpdateTeamMember;
using TimeSheet.Application.TeamMember.Queries.GetTeamMemberById;
using TimeSheet.Application.TeamMember.Queries.GetTeamMembers;

namespace TimeSheet.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/teammembers")]
public class TeamMembersController : ControllerBase
{
    private readonly IMediator Mediator;

    public TeamMembersController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<TeamMembersResponse>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetTeamMembersQuery(), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TeamMemberResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetTeamMemberByIdQuery(id), cancellationToken));


    }

    [HttpPost]
    [Authorize(Roles =Roles.Admin)]
    public async Task<ActionResult<TeamMemberResponse>> Create(CreateTeamMemberRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateTeamMemberCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);

    }
    [HttpDelete("{id:guid}")]
    [Authorize(Roles =Roles.Admin)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteTeamMemberCommand(id), cancellationToken);
        return NoContent();

    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles =Roles.Admin)]
    public async Task<ActionResult> Update(Guid id, UpdateTeamMemberRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UpdateTeamMemberCommand(id, request), cancellationToken);
        return NoContent();

    }

}