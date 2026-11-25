using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Client.Commands.DeleteClient;
using TimeSheet.Core.Repositories;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Tests.Clients.Commands;

[TestFixture]
public class DeleteClientHandlerTests
{
    private DeleteClientTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new DeleteClientTestContext();
    }

    [Test]
    public void Handle_ClientNotFound()
    {
        context.WithClientNotFound();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.CreateCommand(), CancellationToken.None));

        context.RepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Test]
    public async Task Handle_ClientWithoutProjects()
    {
        context.WithClientExisting();

        await context.Handler.Handle(context.CreateCommand(), CancellationToken.None);

        context.RepositoryMock.Verify(r => r.DeleteAsync(context.ClientId), Times.Once);
    }

}

public class DeleteClientTestContext
{
    public Guid ClientId { get; } = Guid.NewGuid();

    public Mock<IClientRepository> RepositoryMock { get; } = new();
    public DeleteClientHandler Handler { get; }

    public DeleteClientTestContext()
    {
        Handler = new DeleteClientHandler(RepositoryMock.Object);
    }

    public DeleteClientCommand CreateCommand()
    {
        return new DeleteClientCommand(ClientId);
    }

    public DeleteClientTestContext WithClientExisting()
    {
        RepositoryMock
            .Setup(r => r.GetByIdAsync(ClientId))
            .ReturnsAsync(new ClientModel(
                ClientId, "VegaIT", "Bulevar Oslobodjenja 1", "Novi Sad",
                "21000", Country.Serbia));

        return this;
    }

    public DeleteClientTestContext WithClientNotFound()
    {
        RepositoryMock
            .Setup(r => r.GetByIdAsync(ClientId))
            .ReturnsAsync((ClientModel?)null);

        return this;
    }
}