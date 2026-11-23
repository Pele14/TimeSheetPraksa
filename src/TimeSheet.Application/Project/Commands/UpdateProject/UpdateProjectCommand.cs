
using MediatR;
using TimeSheet.Application.Requests.ProjectRequests;

namespace TimeSheet.Application.Project.Commands.UpdateProject;


public record UpdateProjectCommand(Guid id, UpdateProjectRequest Request): IRequest;