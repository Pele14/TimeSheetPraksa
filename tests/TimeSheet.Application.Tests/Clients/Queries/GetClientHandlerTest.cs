using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Client.Queries.GetClientById;
using TimeSheet.Core.Repositories;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Tests.Clients.Queries;

[TestFixture]
public class GetClientHandlerTests
{
    private GetClientTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new GetClientTestContext();
    }

    [Test]
    public void Handle_ClientNotFound_ThrowsNotFoundException()
    {
        context.WithClientNotFound();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.CreateQuery(), CancellationToken.None));
    }

    [Test]
    public async Task Handle_ClientFound()
    {
        context.WithClientExisting();

        var result = await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        var Client = context.Client;
        Assert.That(result.Id, Is.EqualTo(Client.Id));
        Assert.That(result.Name, Is.EqualTo(Client.Name));
        Assert.That(result.Address, Is.EqualTo(Client.Address));
        Assert.That(result.City, Is.EqualTo(Client.City));
        Assert.That(result.Zip, Is.EqualTo(Client.Zip));
        Assert.That(result.Country, Is.EqualTo(Client.Country));
    }
}

public class GetClientTestContext
{
    public Guid ClientId { get; } = Guid.NewGuid();
    public ClientModel Client { get; }

    public Mock<IClientRepository> RepositoryMock { get; } = new();
    public GetClientByIdHandler Handler { get; }

    public GetClientTestContext()
    {
        Handler = new GetClientByIdHandler(RepositoryMock.Object);

        Client = new ClientModel(
            ClientId, "VegaIT", "Bulevar Oslobodjenja 1", "Novi Sad",
            "21000", Country.Serbia);
    }

    public GetClientByIdQuery CreateQuery()
    {
        return new GetClientByIdQuery(ClientId);
    }

    public GetClientTestContext WithClientExisting()
    {
        RepositoryMock
            .Setup(r => r.GetByIdAsync(ClientId))
            .ReturnsAsync(Client);

        return this;
    }

    public GetClientTestContext WithClientNotFound()
    {
        RepositoryMock
            .Setup(r => r.GetByIdAsync(ClientId))
            .ReturnsAsync((ClientModel?)null);

        return this;
    }
}