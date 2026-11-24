using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Application.Requests.TimeEntryRequests;
using TimeSheet.Application.TimeEntry.Commands.CreateTimeEntry;
using TimeSheet.Application.TimeEntry.Commands.DeleteTimeEntry;
using TimeSheet.Application.TimeEntry.Commands.UpdateTimeEntry;
using TimeSheet.Application.TimeEntry.Queries.GetTimeEntryById;
using Timesheet.Infrastructure.DBEntities;
using TimeSheet.Application.Responses.TimeEntryResponse;

namespace TimeSheet.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/TimeEntry")]
public class TimeEntriesController : ControllerBase
{
    private readonly IMediator Mediator;

    public TimeEntriesController(IMediator mediator)
    {
        Mediator = mediator;
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TimeEntryResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetTimeEntryByIdQuery(id), cancellationToken));


    }

    [HttpPost]
    public async Task<ActionResult<TimeEntryResponse>> Create(CreateTimeEntryRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateTimeEntryCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);

    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteTimeEntryCommand(id), cancellationToken);
        return NoContent();

    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, UpdateTimeEntryRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UpdateTimeEntryCommand(id, request), cancellationToken);
        return NoContent();

    }

}