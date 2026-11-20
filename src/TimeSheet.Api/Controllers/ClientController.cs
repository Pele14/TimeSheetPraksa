using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Application.Requests.ClientRequests;
using TimeSheet.Application.Client.Commands.CreateClient;
using TimeSheet.Application.Client.Commands.DeleteClient;
using TimeSheet.Application.Client.Commands.UpdateClient;
using TimeSheet.Application.Client.Queries.GetClientById;
using TimeSheet.Application.Client.Queries.GetClients;

namespace TimeSheet.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/client")]
public class ClientController : ControllerBase
{
    private readonly IMediator Mediator;

    public ClientController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ClientResponse>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetClientsQuery(), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClientResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetClientByIdQuery(id), cancellationToken));


    }

    [HttpPost]
    public async Task<ActionResult<ClientResponse>> Create(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateClientCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);

    }
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteClientCommand(id), cancellationToken);
        return NoContent();

    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, UpdateClientRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UpdateClientCommand(id, request), cancellationToken);
        return NoContent();

    }

}