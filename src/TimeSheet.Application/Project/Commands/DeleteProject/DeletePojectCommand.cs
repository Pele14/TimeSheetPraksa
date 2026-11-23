
using MediatR;

namespace TimeSheet.Application.Project.Commands.DeleteProject;


public record DeleteProjectCommand(Guid Id): IRequest;