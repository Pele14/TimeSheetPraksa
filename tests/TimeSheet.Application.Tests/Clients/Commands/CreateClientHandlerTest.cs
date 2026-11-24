using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.ClientRequests;
using TimeSheet.Application.Client.Commands.CreateClient;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Tests.Clients.Commands;



[TestFixture]
public class CreateClientHandlerTests
{
    private CreateClientTestContext context=null!;

    [SetUp]
    public void SetUp()
    {
            context=new CreateClientTestContext();
    }
    
    [Test]   
    public void Handle_NamelAlreadyExists()
    {
        context.WithNameTaken();

        Assert.ThrowsAsync<ConflictException>(() =>
            context.Handler.Handle(context.CreateValidCommand(), CancellationToken.None));

        context.RepositoryMock.Verify(r => r.AddAsync(It.IsAny<ClientModel>()), Times.Never);
    }
    
    [Test]
    public async Task Handle_ValidCommand()
    {
        context.WithNameAvailable();

        var result = await context.Handler.Handle(context.CreateValidCommand(), CancellationToken.None);

        context.RepositoryMock.Verify(r => r.AddAsync(It.IsAny<ClientModel>()), Times.Once);
        Assert.That(result.Name, Is.EqualTo(CreateClientTestContext.Name));
        Assert.That(result.Address, Is.EqualTo("Bulevar Oslobodjenja 1"));
        Assert.That(result.City, Is.EqualTo("Novi Sad"));
        Assert.That(result.Zip, Is.EqualTo("21000"));
        Assert.That(result.Country, Is.EqualTo(Country.Serbia));

    }

}
public class CreateClientTestContext
{
    public const string Name = "VegaIT";

    public Mock<IClientRepository> RepositoryMock { get; } = new();
    public CreateClientHandler Handler { get; }

    public CreateClientTestContext()
    {
        Handler = new CreateClientHandler(RepositoryMock.Object);
    }

    public CreateClientCommand CreateValidCommand()
    {
        return new CreateClientCommand(new CreateClientRequest
        {
            Name = Name,
            Address="Bulevar Oslobodjenja 1",
            City="Novi Sad",
            Zip="21000",
            Country=Country.Serbia
        });
    }

    public CreateClientTestContext WithNameTaken()
    {
        RepositoryMock
            .Setup(r => r.ExistsByNameAsync(Name))
            .ReturnsAsync(true);

        return this;
    }

    public CreateClientTestContext WithNameAvailable()
    {
        RepositoryMock
            .Setup(r => r.ExistsByNameAsync(Name))
            .ReturnsAsync(false);

        return this;
    }
}