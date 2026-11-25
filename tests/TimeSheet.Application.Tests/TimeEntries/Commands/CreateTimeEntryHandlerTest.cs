using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.TimeEntryRequests;
using TimeSheet.Application.TimeEntry.Commands.CreateTimeEntry;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;
using TimeSheet.Domain.Enums;
using TimeSheet.Domain.Exceptions;

namespace TimeSheet.Application.Tests.TimeEntrys.Commands;



[TestFixture]
public class CreateTimeEntryHandlerTests
{
    private CreateTimeEntryTestContext context=null!;

    [SetUp]
    public void SetUp()
    {
            context=new CreateTimeEntryTestContext();
    }
       
    [Test]
    public async Task Handle_ValidCommand()
    {
        context.WithProjectExisting()
               .WithCategoryExisting()
               .WithClientExisting()
               .WithMemberExisting();

        var result = await context.Handler.Handle(context.CreateValidCommand(), CancellationToken.None);

        context.TimeEntryRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TimeEntryModel>()), Times.Once);
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Hours, Is.EqualTo(CreateTimeEntryTestContext.Hours));
        Assert.That(result.ProjectId, Is.EqualTo(context.projectId));
        Assert.That(result.ProjectName, Is.EqualTo(CreateTimeEntryTestContext.ProjectName));
        Assert.That(result.ClientId, Is.EqualTo(context.clinetId));
        Assert.That(result.ClientName, Is.EqualTo(CreateTimeEntryTestContext.ClientName));
        Assert.That(result.CategoryId, Is.EqualTo(context.categoryId));
        Assert.That(result.CategoryName, Is.EqualTo(CreateTimeEntryTestContext.CategoryName));
        Assert.That(result.TeamMemberId, Is.EqualTo(context.memberId));
    }

    [Test]
    public async Task Handle_ProjectNotFound()
    {
        context.WithClientExisting()
               .WithCategoryExisting()
               .WithProjectNotFound()
               .WithMemberExisting();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.CreateValidCommand(),CancellationToken.None));    
        
        context.VerifyTimeEntryNotAdded();
    }

    [Test]
     public async Task Handle_MemberNotFound()
    {
        context.WithProjectExisting()
               .WithCategoryExisting()
               .WithClientExisting()
               .WithMemberNotFound();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.CreateValidCommand(),CancellationToken.None));    
        
        context.VerifyTimeEntryNotAdded();
    }

    [Test]
     public async Task Handle_CategoryNotFound()
    {
        context.WithProjectExisting()
               .WithCategoryNotFound()
               .WithClientExisting()
               .WithMemberExisting();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.CreateValidCommand(),CancellationToken.None));    
        
        context.VerifyTimeEntryNotAdded();
    }

    [Test]
     public async Task Handle_InvalidHours()
    {
        context.WithProjectExisting()
               .WithCategoryExisting()
               .WithClientExisting()
               .WithMemberExisting();

        Assert.ThrowsAsync<DomainException>(() =>
            context.Handler.Handle(context.CreateCommandWithHours(30),CancellationToken.None));    
        
        context.VerifyTimeEntryNotAdded();
    }



}
public class CreateTimeEntryTestContext
{   
    public const string ProjectName="Project";
    public const string ClientName="Aleksa";
    public const string CategoryName="Backend Dev";
    public const double Hours=6;
    public Guid categoryId{get; }=Guid.NewGuid();
    public Guid projectId{get; }=Guid.NewGuid();
    public Guid clinetId{get; }=Guid.NewGuid();
    public Guid memberId{get; }=Guid.NewGuid();

    public Mock<ITimeEntryRepository> TimeEntryRepositoryMock { get; } = new();
    public Mock<IClientRepository> ClientRepositoryMock { get; } = new();
    public Mock<ITeamMemberRepository> TeamMemberRepositoryMock { get; } = new();
    public Mock<ICategoryRepository> CategoryRepositoryMock { get; } = new();
    public Mock<IProjectRepository> ProjectRepositoryMock { get; } = new();
    public CreateTimeEntryHandler Handler { get; }

    public CreateTimeEntryTestContext()
    {
        Handler = new CreateTimeEntryHandler(TimeEntryRepositoryMock.Object,
        ProjectRepositoryMock.Object, ClientRepositoryMock.Object,
        CategoryRepositoryMock.Object, TeamMemberRepositoryMock.Object);
    }
    public CreateTimeEntryCommand CreateValidCommand()
    {   
        return CreateCommandWithHours(Hours);
    }

    public CreateTimeEntryCommand CreateCommandWithHours(double hours)
    {
        return new CreateTimeEntryCommand(new CreateTimeEntryRequest
        {
           Date=new DateOnly(2026, 11, 25),
           Hours=hours,
           Overtime=0.5,
           Description="Description",
           ProjectId=projectId,
           CategoryId=categoryId,
           TeamMemberId=memberId
        });
    }

  
    public CreateTimeEntryTestContext WithClientExisting()
    {
        ClientRepositoryMock
            .Setup(r => r.GetByIdAsync(clinetId))
            .ReturnsAsync(new ClientModel(clinetId, ClientName, 
             "Bulevar", "Novi Sad", "21000", Country.Serbia));
        return this;
    }

    public CreateTimeEntryTestContext WithClientNotFound()
    {
        ClientRepositoryMock
            .Setup(r => r.GetByIdAsync(clinetId))
            .ReturnsAsync((ClientModel?)null);
        return this;
    }

    public CreateTimeEntryTestContext WithMemberExisting()
    {
        TeamMemberRepositoryMock
            .Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync(new TeamMemberModel(memberId, "Aleksa", 
             "aleksa123", "aleksa.pele03@gmail.com",
             "hashed", 40, Role.Admin, MemberStatus.Active));
        return this;
    }

    public CreateTimeEntryTestContext WithMemberNotFound()
    {
        TeamMemberRepositoryMock
            .Setup(r => r.GetByIdAsync(memberId))
            .ReturnsAsync((TeamMemberModel?)null);
        return this;
    }

    public CreateTimeEntryTestContext WithProjectExisting()
    {
        ProjectRepositoryMock
            .Setup(r=> r.GetByIdAsync(projectId))
            .ReturnsAsync(new ProjectModel(projectId, ProjectName, "Descripion", ProjectStatus.Active, clinetId, memberId));
        return this;
    }
     public CreateTimeEntryTestContext WithProjectNotFound()
    {
        ProjectRepositoryMock
            .Setup(r=> r.GetByIdAsync(projectId))
            .ReturnsAsync((ProjectModel?)null);
        return this;
    }

    
    public CreateTimeEntryTestContext WithCategoryExisting()
    {
        CategoryRepositoryMock
            .Setup(r=> r.GetByIdAsync(categoryId))
            .ReturnsAsync(new CategoryModel(categoryId, CategoryName));
        return this;
    }
     public CreateTimeEntryTestContext WithCategoryNotFound()
    {
        CategoryRepositoryMock
            .Setup(r=> r.GetByIdAsync(categoryId))
            .ReturnsAsync((CategoryModel?)null);
        return this;
    }


    public void VerifyTimeEntryNotAdded()
    {
        TimeEntryRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TimeEntryModel>()), Times.Never);
    }

}