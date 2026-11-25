using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Project.Commands.DeleteProject;
using TimeSheet.Core.Repositories;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Tests.Projects.Commands;

[TestFixture]
public class DeleteProjectHandlerTests
{
    private DeleteProjectTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new DeleteProjectTestContext();
    }

    [Test]
    public void Handle_ProjectNotFound()
    {
        context.WithProjectNotFound();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.CreateCommand(), CancellationToken.None));

        context.VerifyProjectNotDeleted();
    }

    [Test]
    public async Task Handle_ProjectWithoutTimeEntries()
    {
        context.WithProjectExisting();

        await context.Handler.Handle(context.CreateCommand(), CancellationToken.None);

        context.ProjectRepositoryMock.Verify(r => r.DeleteAsync(context.ProjectId), Times.Once);
    }



}

public class DeleteProjectTestContext
{
    public Guid ProjectId { get; } = Guid.NewGuid();

    public Mock<IProjectRepository> ProjectRepositoryMock { get; } = new();
    public DeleteProjectHandler Handler { get; }

    public DeleteProjectTestContext()
    {
        Handler = new DeleteProjectHandler(ProjectRepositoryMock.Object);
    }

    public DeleteProjectCommand CreateCommand()
    {
        return new DeleteProjectCommand(ProjectId);
    }

    public DeleteProjectTestContext WithProjectExisting()
    {
        ProjectRepositoryMock
            .Setup(r => r.GetByIdAsync(ProjectId))
            .ReturnsAsync(new ProjectModel(
                ProjectId, "VegaIT", "Description", ProjectStatus.Active,
                Guid.NewGuid(), Guid.NewGuid()));

        return this;
    }

    public DeleteProjectTestContext WithProjectNotFound()
    {
        ProjectRepositoryMock
            .Setup(r => r.GetByIdAsync(ProjectId))
            .ReturnsAsync((ProjectModel?)null);

        return this;
    }



    public void VerifyProjectNotDeleted()
    {
        ProjectRepositoryMock.Verify(r=>r.DeleteAsync(It.IsAny<Guid>()), Times.Never);

    }
}