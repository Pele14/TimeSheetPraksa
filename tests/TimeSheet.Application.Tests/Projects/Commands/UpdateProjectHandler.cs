using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.ProjectRequests;
using TimeSheet.Application.Project.Commands.UpdateProject;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;
using TimeSheet.Domain.Enums;
using TimeSheet.Application.Tests.Clients.Commands;

namespace TimeSheet.Application.Tests.Projects.Commands;



[TestFixture]
public class UpdateProjectHandlerTests
{
    private UpdateProjectTestContext context=null!;

    [SetUp]
    public void SetUp()
    {
            context=new UpdateProjectTestContext();
    }
    
    [Test]   
    public void Handle_ProjectNotFound()
    {
        context.WithProjectNotFound()
               .WithClientExisting()
               .WithLeadExisting();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.UpdateValidCommand(UpdateProjectTestContext.NewName), CancellationToken.None));

        context.VerifyProjectNotUpdated();
    }
    

    [Test]
    public void Handle_NewNameTakenByAnotherProject_ThrowsConflictException()
    {
        context
            .WithProjectExisting()
            .WithNameTaken(UpdateProjectTestContext.NewName)
            .WithClientExisting()
            .WithLeadExisting();

        Assert.ThrowsAsync<ConflictException>(() =>
            context.Handler.Handle(context.UpdateValidCommand(UpdateProjectTestContext.NewName), CancellationToken.None));

        context.VerifyProjectNotUpdated();
    }

    [Test]
    public async Task Handle_SameNameDifferentCase_DoesNotCheckForDuplicate()
    {
        context
            .WithProjectExisting()
            .WithClientExisting()
            .WithLeadExisting();

        await context.Handler.Handle(context.UpdateValidCommand("VEGAIT"), CancellationToken.None);

        context.ProjectRepositoryMock.Verify(r => r.ExistsByNameAsync(It.IsAny<string>()), Times.Never);
        context.ProjectRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ProjectModel>()), Times.Once);
    }

    [Test]
    public async Task Handle_ValidCommand()
    {
        context
               .WithProjectExisting()
               .WithNameAvailable(UpdateProjectTestContext.NewName)
               .WithClientExisting()
               .WithLeadExisting();

        await context.Handler.Handle(context.UpdateValidCommand(UpdateProjectTestContext.NewName), CancellationToken.None);

        context.ProjectRepositoryMock.Verify(r => r.UpdateAsync(It.Is<ProjectModel>( p=>
            p.Id==context.ProjectId &&
            p.Name==UpdateProjectTestContext.NewName &&
            p.Description=="Nova verzija" &&
            p.ProjectStatus==ProjectStatus.Archived &&
            p.ClientId== context.NewClinetId &&
            p.TeamMemberId==context.NewLeadId ))
        
        
            , Times.Once);
    }

    [Test]

    public async Task Handle_ClientNotFound()
    {
        context
            .WithProjectExisting()
            .WithNameAvailable(UpdateProjectTestContext.NewName)
            .WithClientNotFound()
            .WithLeadExisting();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.UpdateValidCommand(UpdateProjectTestContext.NewName),CancellationToken.None));    
        
        context.VerifyProjectNotUpdated();
    }

    [Test]
     public async Task Handle_LeadNotFound()
    {
        context
            .WithProjectExisting()
            .WithNameAvailable(UpdateProjectTestContext.NewName)
            .WithClientExisting()
            .WithLeadNotFound();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.UpdateValidCommand(UpdateClientTestContext.NewName),CancellationToken.None));    
        
        context.VerifyProjectNotUpdated();
    }

}
public class UpdateProjectTestContext
{
    public const string ExistingName = "VegaIT";
    public const string NewName="Nina Media";
    public Guid ProjectId{get; }=Guid.NewGuid();
    public Guid NewClinetId{get; }=Guid.NewGuid();
    public Guid NewLeadId{get; }=Guid.NewGuid();

    public Mock<IProjectRepository> ProjectRepositoryMock { get; } = new();
    public Mock<IClientRepository> ClientRepositoryMock { get; } = new();
    public Mock<ITeamMemberRepository> TeamMemberRepositoryMock { get; } = new();
    public UpdateProjectHandler Handler { get; }

    public UpdateProjectTestContext()
    {
        Handler = new UpdateProjectHandler(ProjectRepositoryMock.Object,
        ClientRepositoryMock.Object, TeamMemberRepositoryMock.Object);
    }

    public UpdateProjectCommand UpdateValidCommand(string name)
    {
        return new UpdateProjectCommand(ProjectId, new UpdateProjectRequest
        {
            Name = name,
            Description="Nova verzija",
            Status=ProjectStatus.Archived,
            ClientId=NewClinetId,
            LeadID=NewLeadId
        });
    }
    public UpdateProjectTestContext WithProjectExisting()
    {
        ProjectRepositoryMock
            .Setup(r=>r.GetByIdAsync(ProjectId))
            .ReturnsAsync(new ProjectModel(
                ProjectId, ExistingName, "stari opis", ProjectStatus.Active,
                Guid.NewGuid(), Guid.NewGuid()
            ));
        return this;
    }

    public UpdateProjectTestContext WithProjectNotFound()
    {
        ProjectRepositoryMock
            .Setup(r=>r.GetByIdAsync(ProjectId))
            .ReturnsAsync((ProjectModel?)null);
        return this;
    }

    public UpdateProjectTestContext WithNameTaken(string name)
    {
        ProjectRepositoryMock
            .Setup(r => r.ExistsByNameAsync(name))
            .ReturnsAsync(true);

        return this;
    }

    public UpdateProjectTestContext WithNameAvailable(string name)
    {
        ProjectRepositoryMock
            .Setup(r => r.ExistsByNameAsync(name))
            .ReturnsAsync(false);

        return this;
    }

    public UpdateProjectTestContext WithClientExisting()
    {
        ClientRepositoryMock
            .Setup(r => r.GetByIdAsync(NewClinetId))
            .ReturnsAsync(new ClientModel(NewClinetId, NewName, 
             "Bulevar", "Novi Sad", "21000", Country.Serbia));
        return this;
    }

    public UpdateProjectTestContext WithClientNotFound()
    {
        ClientRepositoryMock
            .Setup(r => r.GetByIdAsync(NewClinetId))
            .ReturnsAsync((ClientModel?)null);
        return this;
    }

    public UpdateProjectTestContext WithLeadExisting()
    {
        TeamMemberRepositoryMock
            .Setup(r => r.GetByIdAsync(NewLeadId))
            .ReturnsAsync(new TeamMemberModel(NewLeadId, "Aleksa", 
             "aleksa123", "aleksa.pele03@gmail.com",
             "hashed", 40, Role.Admin, MemberStatus.Active));
        return this;
    }

    public UpdateProjectTestContext WithLeadNotFound()
    {
        TeamMemberRepositoryMock
            .Setup(r => r.GetByIdAsync(NewLeadId))
            .ReturnsAsync((TeamMemberModel?)null);
        return this;
    }

    public void VerifyProjectNotUpdated()
    {
        ProjectRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<ProjectModel>()), Times.Never);
    }

}