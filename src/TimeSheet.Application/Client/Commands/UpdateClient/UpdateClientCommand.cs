
using MediatR;
using TimeSheet.Application.Requests.ClientRequests;

namespace TimeSheet.Application.Client.Commands.UpdateClient;


public record UpdateClientCommand(Guid id, UpdateClientRequest Request): IRequest;