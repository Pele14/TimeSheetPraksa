using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.ClientRequests;
using TimeSheet.Application.Client.Commands.UpdateClient;
using TimeSheet.Core.Repositories;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Tests.Clients.Commands;

[TestFixture]
public class UpdateClientHandlerTests
{
    private UpdateClientTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new UpdateClientTestContext();
    }

    [Test]
    public void Handle_ClientNotFound_ThrowsNotFoundException()
    {
        context.WithClientNotFound();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(
                context.CreateCommand(UpdateClientTestContext.NewName),
                CancellationToken.None));

        context.RepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ClientEntity>()), Times.Never);
    }

    [Test]
    public void Handle_NewNameTakenByAnotherClient_ThrowsConflictException()
    {
        context
            .WithClientExisting()
            .WithNameTaken(UpdateClientTestContext.NewName);

        Assert.ThrowsAsync<ConflictException>(() =>
            context.Handler.Handle(
                context.CreateCommand(UpdateClientTestContext.NewName),
                CancellationToken.None));

        context.RepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ClientEntity>()), Times.Never);
    }

    [Test]
    public async Task Handle_SameName_DoesNotCheckForDuplicate()
    {
        context.WithClientExisting();

        await context.Handler.Handle(
            context.CreateCommand("VEGAIT"),
            CancellationToken.None);

        context.RepositoryMock.Verify(r => r.ExistsByNameAsync(It.IsAny<string>()), Times.Never);
        context.RepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ClientEntity>()), Times.Once);
    }

    [Test]
    public async Task Handle_ValidCommand_UpdatesClientWithNewValues()
    {
        context
            .WithClientExisting()
            .WithNameAvailable(UpdateClientTestContext.NewName);

        await context.Handler.Handle(
            context.CreateCommand(UpdateClientTestContext.NewName),
            CancellationToken.None);

        context.RepositoryMock.Verify(r => r.UpdateAsync(It.Is<ClientEntity>(m =>
            m.Id == context.ClientId &&
            m.Name == UpdateClientTestContext.NewName &&
            m.Address == "Svetozara Corovica 11" &&
            m.City=="Beograd" &&
            m.Zip=="11000" &&
            m.Country==Country.Croatia)),
            Times.Once);
    }
}

public class UpdateClientTestContext
{
    public const string ExistingName = "VegaIT";
    public const string NewName="Continental";

    public Guid ClientId { get; } = Guid.NewGuid();

    public Mock<IClientRepository> RepositoryMock { get; } = new();
    public UpdateClientHandler Handler { get; }

    public UpdateClientTestContext()
    {
        Handler = new UpdateClientHandler(RepositoryMock.Object);
    }

    public UpdateClientCommand CreateCommand(string name)
    {
        return new UpdateClientCommand(ClientId, new UpdateClientRequest
        {
            Name = name,
            Address="Svetozara Corovica 11",
            City="Beograd",
            Zip="11000",
            country=Country.Croatia
        });
    }

    public UpdateClientTestContext WithClientExisting()
    {
        RepositoryMock
            .Setup(r => r.GetByIdAsync(ClientId))
            .ReturnsAsync(new ClientEntity(
                ClientId, ExistingName, "Bulevar Oslobodjenja 1", "Novi Sad", "21000", Country.Serbia));

        return this;
    }

    public UpdateClientTestContext WithClientNotFound()
    {
        RepositoryMock
            .Setup(r => r.GetByIdAsync(ClientId))
            .ReturnsAsync((ClientEntity?)null);

        return this;
    }

    public UpdateClientTestContext WithNameTaken(string name)
    {
        RepositoryMock
            .Setup(r => r.ExistsByNameAsync(name))
            .ReturnsAsync(true);

        return this;
    }

    public UpdateClientTestContext WithNameAvailable(string name)
    {
        RepositoryMock
            .Setup(r => r.ExistsByNameAsync(name))
            .ReturnsAsync(false);

        return this;
    }
}