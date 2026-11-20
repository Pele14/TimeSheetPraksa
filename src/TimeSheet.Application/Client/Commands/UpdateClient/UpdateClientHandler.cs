using MediatR;
using TimeSheet.Application.Common.Exceptions;

using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.Client.Commands.UpdateClient;


public class UpdateClientHandler : IRequestHandler<UpdateClientCommand>
{
    private readonly IClientRepository ClientRepository;

    public UpdateClientHandler(IClientRepository clientRepository)
    {
        ClientRepository = clientRepository;

    }

    public async Task Handle(UpdateClientCommand command, CancellationToken cancellationToken)
    {
        var entity = await ClientRepository.GetByIdAsync(command.id) ??
            throw new NotFoundException("Client doesnt exist");

        var request = command.Request;
        var namechanged=!string.Equals(entity.Name, request.Name.Trim(), StringComparison.OrdinalIgnoreCase);
        if (namechanged && await ClientRepository.ExistsByNameAsync(request.Name))
            throw new ConflictException("Email is already in use");
        entity.Update(request.Name, request.Address, request.City, request.Zip, request.country);
        await ClientRepository.UpdateAsync(entity);
    }
}