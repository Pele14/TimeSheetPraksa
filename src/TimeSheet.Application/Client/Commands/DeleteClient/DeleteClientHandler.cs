
using MediatR;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.Client.Commands.DeleteClient;


public class DeleteClientHandler : IRequestHandler<DeleteClientCommand>
{
    private readonly IClientRepository ClientRepository;

    public DeleteClientHandler(IClientRepository clientRepository)
    {
        ClientRepository = clientRepository;
    }



    public async Task Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id;
        _ = await ClientRepository.GetByIdAsync(id) 
            ?? throw new NotFoundException("Email doesn't exist");
        await ClientRepository.DeleteAsync(id);

    }
}