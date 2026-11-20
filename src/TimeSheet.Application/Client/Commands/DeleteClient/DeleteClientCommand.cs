
using MediatR;

namespace TimeSheet.Application.Client.Commands.DeleteClient;


public record DeleteClientCommand(Guid Id): IRequest;