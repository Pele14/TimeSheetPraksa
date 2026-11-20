
using MediatR;
using TimeSheet.Application.Requests.ClientRequests;

namespace TimeSheet.Application.Client.Commands.CreateClient;


public record CreateClientCommand(CreateClientRequest Request) : IRequest<ClientResponse>;
