using MediatR;
using TimeSheet.Application.Requests.ProjectResponses;

namespace TimeSheet.Application.Project.Queries.GetProjectById;

public record GetProjectByIdQuery(Guid Id): IRequest<ProjectResponse>;