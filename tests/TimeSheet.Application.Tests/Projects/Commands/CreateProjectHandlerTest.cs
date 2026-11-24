using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.ProjectRequests;
using TimeSheet.Application.Project.Commands.CreateProject;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Tests.Projects.Commands;



[TestFixture]
public class CreateProjectHandlerTests
{
    private CreateProjectTestContext context=null!;

    [SetUp]
    public void SetUp()
    {
            context=new CreateProjectTestContext();
    }
    
    [Test]   
    public void Handle_NamelAlreadyExists()
    {
        context.WithNameTaken()
               .WithClientExisting()
               .WithLeadExisting();

        Assert.ThrowsAsync<ConflictException>(() =>
            context.Handler.Handle(context.CreateValidCommand(), CancellationToken.None));

        context.VerifyProjectNotAdded();
    }
    
    [Test]
    public async Task Handle_ValidCommand()
    {
        context.WithNameAvailable()
               .WithClientExisting()
               .WithLeadExisting();

        var result = await context.Handler.Handle(context.CreateValidCommand(), CancellationToken.None);

        context.ProjectRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ProjectModel>()), Times.Once);
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Name, Is.EqualTo(CreateProjectTestContext.Name));
        Assert.That(result.ClientId, Is.EqualTo(context.clinetId));
        Assert.That(result.ClientName, Is.EqualTo(CreateProjectTestContext.ClientName));
        Assert.That(result.LeadId, Is.EqualTo(context.leadId));
        Assert.That(result.Status, Is.EqualTo(ProjectStatus.Active));

    }

    [Test]

    public async Task Handle_ClientNotFound()
    {
        context
            .WithNameAvailable()
            .WithClientNotFound()
            .WithLeadExisting();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.CreateValidCommand(),CancellationToken.None));    
        
        context.VerifyProjectNotAdded();
    }

     public async Task Handle_LeadNotFound()
    {
        context
            .WithNameAvailable()
            .WithClientExisting()
            .WithLeadNotFound();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.CreateValidCommand(),CancellationToken.None));    
        
        context.VerifyProjectNotAdded();
    }

}
public class CreateProjectTestContext
{
    public const string Name = "VegaIT";
    public const string ClientName="Nina Media";
    public Guid clinetId{get; }=Guid.NewGuid();
    public Guid leadId{get; }=Guid.NewGuid();

    public Mock<IProjectRepository> ProjectRepositoryMock { get; } = new();
    public Mock<IClientRepository> ClientRepositoryMock { get; } = new();
    public Mock<ITeamMemberRepository> TeamMemberRepositoryMock { get; } = new();
    public CreateProjectHandler Handler { get; }

    public CreateProjectTestContext()
    {
        Handler = new CreateProjectHandler(ProjectRepositoryMock.Object,
        ClientRepositoryMock.Object, TeamMemberRepositoryMock.Object);
    }

    public CreateProjectCommand CreateValidCommand()
    {
        return new CreateProjectCommand(new CreateProjectRequest
        {
            Name = Name,
            Description="New project",
            ClientId=clinetId,
            LeadID=leadId
        });
    }

    public CreateProjectTestContext WithNameTaken()
    {
        ProjectRepositoryMock
            .Setup(r => r.ExistsByNameAsync(Name))
            .ReturnsAsync(true);

        return this;
    }

    public CreateProjectTestContext WithNameAvailable()
    {
        ProjectRepositoryMock
            .Setup(r => r.ExistsByNameAsync(Name))
            .ReturnsAsync(false);

        return this;
    }

    public CreateProjectTestContext WithClientExisting()
    {
        ClientRepositoryMock
            .Setup(r => r.GetByIdAsync(clinetId))
            .ReturnsAsync(new ClientModel(clinetId, ClientName, 
             "Bulevar", "Novi Sad", "21000", Country.Serbia));
        return this;
    }

    public CreateProjectTestContext WithClientNotFound()
    {
        ClientRepositoryMock
            .Setup(r => r.GetByIdAsync(clinetId))
            .ReturnsAsync((ClientModel?)null);
        return this;
    }

    public CreateProjectTestContext WithLeadExisting()
    {
        TeamMemberRepositoryMock
            .Setup(r => r.GetByIdAsync(leadId))
            .ReturnsAsync(new TeamMemberModel(leadId, "Aleksa", 
             "aleksa123", "aleksa.pele03@gmail.com",
             "hashed", 40, Role.Admin, MemberStatus.Active));
        return this;
    }

    public CreateProjectTestContext WithLeadNotFound()
    {
        TeamMemberRepositoryMock
            .Setup(r => r.GetByIdAsync(leadId))
            .ReturnsAsync((TeamMemberModel?)null);
        return this;
    }

    public void VerifyProjectNotAdded()
    {
        ProjectRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ProjectModel>()), Times.Never);
    }

}