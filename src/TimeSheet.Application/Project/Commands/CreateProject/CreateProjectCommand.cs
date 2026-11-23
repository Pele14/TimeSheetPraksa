using MediatR;
using TimeSheet.Application.Requests.ProjectRequests;
using TimeSheet.Application.Requests.ProjectResponses;

namespace TimeSheet.Application.Project.Commands.CreateProject;


public record CreateProjectCommand(CreateProjectRequest Request) : IRequest<ProjectResponse>;
