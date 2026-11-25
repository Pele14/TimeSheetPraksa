using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.TimeEntry.Queries.GetTimeEntryById;
using TimeSheet.Core.Repositories;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Tests.TimeEntrys.Queries;

[TestFixture]
public class GetTimeEntryHandlerTests
{
    private GetTimeEntryTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new GetTimeEntryTestContext();
    }

    [Test]
    public void Handle_TimeEntryNotFound_ThrowsNotFoundException()
    {
        context.WithTimeEntryNotFound()
               .WithProjectExisting()
               .WithClientExisting()
               .WithCategoryExisting();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.GetQuery(), CancellationToken.None));
    }

    [Test]
    public async Task Handle_TimeEntryFound()
    {
        context.WithTimeEntryExisting()
               .WithClientExisting()
               .WithCategoryExisting()
               .WithProjectExisting();

        var result = await context.Handler.Handle(context.GetQuery(), CancellationToken.None);

        Assert.That(result.Id, Is.EqualTo(context.timeEntryId));
        Assert.That(result.Hours, Is.EqualTo(GetTimeEntryTestContext.Hours));
        Assert.That(result.ProjectId, Is.EqualTo(context.projectId));
        Assert.That(result.ProjectName, Is.EqualTo(GetTimeEntryTestContext.ProjectName));
        Assert.That(result.ClientId, Is.EqualTo(context.clientId));
        Assert.That(result.ClientName, Is.EqualTo(GetTimeEntryTestContext.ClientName));
        Assert.That(result.CategoryId, Is.EqualTo(context.categoryId));
        Assert.That(result.CategoryName, Is.EqualTo(GetTimeEntryTestContext.CategoryName));
        Assert.That(result.TeamMemberId, Is.EqualTo(context.memberId));
    }

    [Test]
    public async Task Handle_MissingClient()
    {
        context.WithTimeEntryExisting()
               .WithProjectExisting()
               .WithClientNotFound()
               .WithCategoryExisting();
        Assert.ThrowsAsync<NotFoundException>(()=>
            context.Handler.Handle(context.GetQuery(), CancellationToken.None));


    }

    [Test]
    public async Task Handle_MissingProject()
    {
        context.WithTimeEntryExisting()
               .WithProjectNotFound()
               .WithClientExisting()
               .WithCategoryExisting();
        Assert.ThrowsAsync<NotFoundException>(()=>
            context.Handler.Handle(context.GetQuery(), CancellationToken.None));
    }

    [Test]
    public async Task Handle_MissingCategory()
    {
        context.WithTimeEntryExisting()
               .WithProjectExisting()
               .WithClientExisting()
               .WithCategoryNotFound();
        Assert.ThrowsAsync<NotFoundException>(()=>
            context.Handler.Handle(context.GetQuery(), CancellationToken.None));
    }
}

public class GetTimeEntryTestContext
{  public const string ProjectName="Project";
    public const string ClientName="Aleksa";
    public const string CategoryName="Backend Dev";
    public const double Hours=7.5;
    public Guid timeEntryId{get; }=Guid.NewGuid();
    public Guid categoryId{get; }=Guid.NewGuid();
    public Guid projectId{get; }=Guid.NewGuid();
    public Guid clientId{get; }=Guid.NewGuid();
    public Guid memberId{get; }=Guid.NewGuid();

    public Mock<ITimeEntryRepository> TimeEntryRepositoryMock { get; } = new();
    public Mock<IClientRepository> ClientRepositoryMock { get; } = new();
    public Mock<ICategoryRepository> CategoryRepositoryMock { get; } = new();
    public Mock<IProjectRepository> ProjectRepositoryMock { get; } = new();
    public GetTimeEntryByIdHandler Handler { get; }

    public GetTimeEntryTestContext()
    {
        Handler = new GetTimeEntryByIdHandler(TimeEntryRepositoryMock.Object, ProjectRepositoryMock.Object, ClientRepositoryMock.Object, CategoryRepositoryMock.Object);

    }

    public GetTimeEntryByIdQuery GetQuery()
    {
        return new GetTimeEntryByIdQuery(timeEntryId);
    }

    public GetTimeEntryTestContext WithTimeEntryExisting()
    {
        TimeEntryRepositoryMock
            .Setup(r => r.GetByIdAsync(timeEntryId))
            .ReturnsAsync(new TimeEntryModel(timeEntryId, new DateOnly(2026, 11, 25),
            Hours, 0.5, "Opis",
            project: projectId,
            category: categoryId,
            teammember: memberId));

        return this;
    }

    public GetTimeEntryTestContext WithTimeEntryNotFound()
    {
        TimeEntryRepositoryMock
            .Setup(r => r.GetByIdAsync(timeEntryId))
            .ReturnsAsync((TimeEntryModel?)null);

        return this;
    }

    public GetTimeEntryTestContext WithClientExisting()
    {
        ClientRepositoryMock
            .Setup(r => r.GetByIdAsync(clientId))
            .ReturnsAsync(new ClientModel(
                clientId, ClientName, "Bulevar 10", "Novi Sad", "21000",Country.Serbia
            ));

        return this;
    }

    public GetTimeEntryTestContext WithClientNotFound()
    {
        ClientRepositoryMock
            .Setup(r => r.GetByIdAsync(clientId))
            .ReturnsAsync((ClientModel?)null);

        return this;
    }

     public GetTimeEntryTestContext WithProjectExisting()
    {
        ProjectRepositoryMock
            .Setup(r=> r.GetByIdAsync(projectId))
            .ReturnsAsync(new ProjectModel(projectId, ProjectName, "Descripion", ProjectStatus.Active, clientId, memberId));
        return this;
    }
     public GetTimeEntryTestContext WithProjectNotFound()
    {
        ProjectRepositoryMock
            .Setup(r=> r.GetByIdAsync(projectId))
            .ReturnsAsync((ProjectModel?)null);
        return this;
    }

    
    public GetTimeEntryTestContext WithCategoryExisting()
    {
        CategoryRepositoryMock
            .Setup(r=> r.GetByIdAsync(categoryId))
            .ReturnsAsync(new CategoryModel(categoryId, CategoryName));
        return this;
    }
     public GetTimeEntryTestContext WithCategoryNotFound()
    {
        CategoryRepositoryMock
            .Setup(r=> r.GetByIdAsync(categoryId))
            .ReturnsAsync((CategoryModel?)null);
        return this;
    }

}