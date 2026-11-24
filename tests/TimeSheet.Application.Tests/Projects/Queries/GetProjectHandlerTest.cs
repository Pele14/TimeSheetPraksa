using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Project.Queries.GetProjectById;
using TimeSheet.Core.Repositories;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Tests.Projects.Queries;

[TestFixture]
public class GetProjectHandlerTests
{
    private GetProjectTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new GetProjectTestContext();
    }

    [Test]
    public void Handle_ProjectNotFound_ThrowsNotFoundException()
    {
        context.WithProjectNotFound();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.CreateQuery(), CancellationToken.None));
    }

    [Test]
    public async Task Handle_ProjectFound()
    {
        context.WithProjectExisting()
               .WithClientExisting();

        var result = await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        var project = context.Project;
        Assert.That(result.Id, Is.EqualTo(project.Id));
        Assert.That(result.Name, Is.EqualTo(project.Name));
        Assert.That(result.Description, Is.EqualTo(project.Description));
        Assert.That(result.Status, Is.EqualTo(project.ProjectStatus));
        Assert.That(result.ClientId, Is.EqualTo(project.ClientId));
        Assert.That(result.ClientName, Is.EqualTo(GetProjectTestContext.ClientName));
        Assert.That(result.LeadId, Is.EqualTo(project.TeamMemberId));
    }

    [Test]
    public async Task Handle_MissingClient()
    {
        context
            .WithProjectExisting()
            .WithClientNotFound();
        var result=await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);
        Assert.That(result.ClientName, Is.Empty);

    }
}

public class GetProjectTestContext
{
    public const string ClientName="VegaIT";
    public Guid ProjectId { get; } = Guid.NewGuid();
    public Guid ClientId{ get;}=Guid.NewGuid();
    public ProjectModel Project { get; }

    public Mock<IProjectRepository> ProjectRepositoryMock { get; } = new();
    public Mock<IClientRepository> ClientRepositoryMock { get; } = new();
    public GetProjectByIdHandler Handler { get; }

    public GetProjectTestContext()
    {
        Handler = new GetProjectByIdHandler(ProjectRepositoryMock.Object, ClientRepositoryMock.Object);

        Project = new ProjectModel(
            ProjectId, "VegaIT", "Opis", ProjectStatus.Active,
            ClientId, Guid.NewGuid());
    }

    public GetProjectByIdQuery CreateQuery()
    {
        return new GetProjectByIdQuery(ProjectId);
    }

    public GetProjectTestContext WithProjectExisting()
    {
        ProjectRepositoryMock
            .Setup(r => r.GetByIdAsync(ProjectId))
            .ReturnsAsync(Project);

        return this;
    }

    public GetProjectTestContext WithProjectNotFound()
    {
        ProjectRepositoryMock
            .Setup(r => r.GetByIdAsync(ProjectId))
            .ReturnsAsync((ProjectModel?)null);

        return this;
    }

    public GetProjectTestContext WithClientExisting()
    {
        ClientRepositoryMock
            .Setup(r => r.GetByIdAsync(ClientId))
            .ReturnsAsync(new ClientModel(
                ClientId, ClientName, "Bulevar 10", "Novi Sad", "21000",Country.Serbia
            ));

        return this;
    }

    public GetProjectTestContext WithClientNotFound()
    {
        ClientRepositoryMock
            .Setup(r => r.GetByIdAsync(ClientId))
            .ReturnsAsync((ClientModel?)null);

        return this;
    }
}