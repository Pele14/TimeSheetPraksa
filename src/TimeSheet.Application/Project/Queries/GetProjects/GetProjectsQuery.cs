
using MediatR;
using TimeSheet.Application.Responses.ProjectResponses;

namespace TimeSheet.Application.Project.Queries.GetProjects;

public record GetProjectsQuery(): IRequest<ProjectsResponse>;