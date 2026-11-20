using MediatR;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Responses.ClientResponses;
using TimeSheet.Core.Repositories;
using TimeSheet.Application.Requests.ClientRequests;

namespace TimeSheet.Application.Client.Queries.GetClientById;


public class GetClientByIdHandler : IRequestHandler<GetClientByIdQuery, ClientResponse>
{
    private readonly IClientRepository ClientRepository;

    public GetClientByIdHandler(IClientRepository clientRepository)
    {
        ClientRepository = clientRepository;

    }



    public async Task<ClientResponse> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var Id = request.Id;

        var entity = await ClientRepository.GetByIdAsync(Id) ??
        throw new NotFoundException("Client doesnt exist");


        return entity.ToResponse();

    }

}
