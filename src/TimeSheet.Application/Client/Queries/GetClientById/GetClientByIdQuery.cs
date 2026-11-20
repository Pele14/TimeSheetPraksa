using MediatR;
using TimeSheet.Application.Requests.ClientRequests;

namespace TimeSheet.Application.Client.Queries.GetClientById;

public record GetClientByIdQuery(Guid Id): IRequest<ClientResponse>;