using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Client.Queries.GetClients;
using TimeSheet.Core.Repositories;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Tests.Clients.Queries;

[TestFixture]
public class GetAllClientsHandlerTests
{
    private GetAllClientTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new GetAllClientTestContext();
    }

    [Test]
    public async Task Handle_ClientsExist()
    {
        context.WithClients();

        var result = await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        Assert.That(result.Clients, Has.Count.EqualTo(2));
        Assert.That(result.Clients[0].Id, Is.EqualTo(context.Clients[0].Id));
        Assert.That(result.Clients[1].Id, Is.EqualTo(context.Clients[1].Id));
        Assert.That(result.Clients[0].Name, Is.EqualTo(context.Clients[0].Name));
        Assert.That(result.Clients[1].Name, Is.EqualTo(context.Clients[1].Name));
    }

    [Test]
    public async Task Handle_NoClients()
    {
        context.WithNoClients();

        var result = await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        Assert.That(result.Clients, Is.Not.Null);
        Assert.That(result.Clients, Is.Empty);
    }
}

public class GetAllClientTestContext
{
    public List<ClientEntity> Clients { get; } = new()
    {
        new ClientEntity(Guid.NewGuid(), "VegaIT", "BulevarOslobodjenja 1", "Novi Sad", "21000", Country.Serbia),
        new ClientEntity(Guid.NewGuid(), "Continental", "Bulevar Evrope 6", "Beograd", "11000", Country.Austria)
    };

    public Mock<IClientRepository> RepositoryMock { get; } = new();
    public GetClientsHandler Handler { get; }

    public GetAllClientTestContext()
    {
        Handler = new GetClientsHandler(RepositoryMock.Object);
    }

    public GetClientsQuery CreateQuery()
    {
        return new GetClientsQuery();
    }

    public GetAllClientTestContext WithClients()
    {
        RepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(Clients);

        return this;
    }

    public GetAllClientTestContext WithNoClients()
    {
        RepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<ClientEntity>());

        return this;
    }

}