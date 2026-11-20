
using System.IO.Compression;
using MediatR;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.ClientRequests;
using TimeSheet.Application.Responses.ClientResponses;
using TimeSheet.Application.Responses.TeamMemberResponses;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;

namespace TimeSheet.Application.Client.Commands.CreateClient;


public class CreateClientHandler : IRequestHandler<CreateClientCommand, ClientResponse>
{
    private readonly IClientRepository ClientRepository;
  

    public CreateClientHandler(IClientRepository clientRepository)
    {
        ClientRepository = clientRepository;
    }


    public async Task<ClientResponse> Handle(CreateClientCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var name = request.Name;
        if (await ClientRepository.ExistsByNameAsync(name))
        {
            throw new ConflictException("Name already exists");
        }

        var member = new ClientEntity(Guid.NewGuid(), request.Name,
             request.Address,
             request.City, request.Zip,
             request.Country);

        await ClientRepository.AddAsync(member);


        return member.ToResponse();
    }

}