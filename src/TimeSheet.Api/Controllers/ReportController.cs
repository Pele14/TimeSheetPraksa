
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Application.Report.Queries.Getreport;
using TimeSheet.Application.Requests.RepostRequest;
using TimeSheet.Application.Responses.RepostResponses;

namespace TimeSheet.Api.Controllers;

[ApiController]
[Route("api/Report")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator=mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ReportResponse>> Get([FromQuery]GetReportRequest request, CancellationToken cancellationToken)
    {

        return Ok(await _mediator.Send(new GetReportQuery(request), cancellationToken));

    }
}