using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Project.Queries.GetProjects;
using TimeSheet.Core.Repositories;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Tests.Projects.Queries;

[TestFixture]
public class GetAllProjectsHandlerTests
{
    private GetAllProjectTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new GetAllProjectTestContext();
    }

    [Test]
    public async Task Handle_ProjectsExist()
    {
        context.WithProjects()
               .WithClients();

        var result = await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        Assert.That(result.Projects, Has.Count.EqualTo(2));
        var vega=result.Projects.Single(p => p.Name== "VegaIT");
        var continental=result.Projects.Single(p=> p.Name=="Continental");
        Assert.That(vega.ClientName, Is.EqualTo("Aleksa"));
        Assert.That(continental.ClientName, Is.EqualTo("Marko"));
    }

    [Test]
    public async Task Handle_NoProjects()
    {
        context.WithNoProjects()
              .WithClients();

        var result = await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        Assert.That(result.Projects, Is.Not.Null);
        Assert.That(result.Projects, Is.Empty);
    }
    [Test]
    public async Task Handle_NoClient()
    {
        context.WithProjects()
              .WithNoClients();

        var result = await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        Assert.That(result.Projects, Has.Count.EqualTo(2));

        Assert.That(result.Projects.All(p => p.ClientName==string.Empty), Is.True);
    }
}

public class GetAllProjectTestContext
{
    public Guid id1{get; }=Guid.NewGuid();
    public Guid id2{get; }=Guid.NewGuid();
    public Mock<IProjectRepository> ProjectRepositoryMock { get; } = new();
    public Mock<IClientRepository> ClientRepositoryMock { get; } = new();
    public GetProjectsHandler Handler { get; }

    public GetAllProjectTestContext()
    {
        Handler = new GetProjectsHandler(ProjectRepositoryMock.Object, ClientRepositoryMock.Object);
    }

    public GetProjectsQuery CreateQuery()
    {
        return new GetProjectsQuery();
    }

    public GetAllProjectTestContext WithProjects()
    {
        ProjectRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<ProjectModel>
            {
                new(Guid.NewGuid(), "VegaIT", "description 1", ProjectStatus.Active, id1, Guid.NewGuid()),
                new(Guid.NewGuid(), "Continental", "description 2", ProjectStatus.Active, id2, Guid.NewGuid())

            });

        return this;
    }

    public GetAllProjectTestContext WithNoProjects()
    {
        ProjectRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<ProjectModel>());

        return this;
    }

     public GetAllProjectTestContext WithClients()
    {
        ClientRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<ClientModel>
            {
               new(id1, "Aleksa", "Bulevar 1", "Novi Sad", "21000", Country.Serbia),
               new(id2, "Marko", "Svetozara Corovica 11", "Novi Sad", "21000", Country.Austria) 


            });

        return this;
    }

      public GetAllProjectTestContext WithNoClients()
    {
        ClientRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<ClientModel>());

        return this;
    }

}