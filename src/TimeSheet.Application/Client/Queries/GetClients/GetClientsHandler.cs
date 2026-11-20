using MediatR;
using TimeSheet.Application.Client.Queries.GetClients;
using TimeSheet.Application.Responses.ClientResponses;
using TimeSheet.Application.Responses.TeamMemberResponses;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.Client.Queries.GetClients;



public class GetClientsHandler : IRequestHandler<GetClientsQuery, ClientsResponse>
{
    private readonly IClientRepository ClientRepository;

    public GetClientsHandler(IClientRepository clientRepository)
    {
        ClientRepository = clientRepository;
    }
    public async Task<ClientsResponse> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var members = await ClientRepository.GetAllAsync();

        return new ClientsResponse
        {
           Clients= members.Select(m => m.ToResponse()).ToList()


        };
    }
}