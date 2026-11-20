
using MediatR;
using TimeSheet.Application.Responses.ClientResponses;

namespace TimeSheet.Application.Client.Queries.GetClients;

public record GetClientsQuery(): IRequest<ClientsResponse>;