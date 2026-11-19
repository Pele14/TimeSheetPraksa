using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.TeamMember.Commands.DeleteTeamMember;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.Tests.TeamMembers.Commands;

[TestFixture]
public class DeleteTeamMemberHandlerTests
{
    private DeleteTeamMemberTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new DeleteTeamMemberTestContext();
    }

    [Test]
    public void Handle_MemberNotFound()
    {
        context.WithMemberNotFound();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.CreateCommand(), CancellationToken.None));

        context.RepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Test]
    public async Task Handle_MemberFound()
    {
        context.WithMemberExisting();

        await context.Handler.Handle(context.CreateCommand(), CancellationToken.None);

        context.RepositoryMock.Verify(r => r.DeleteAsync(context.MemberId), Times.Once);
    }
}

public class DeleteTeamMemberTestContext
{
    public Guid MemberId { get; } = Guid.NewGuid();

    public Mock<ITeamMemberRepository> RepositoryMock { get; } = new();
    public DeleteTeamMemberHandler Handler { get; }

    public DeleteTeamMemberTestContext()
    {
        Handler = new DeleteTeamMemberHandler(RepositoryMock.Object);
    }

    public DeleteTeamMemberCommand CreateCommand()
    {
        return new DeleteTeamMemberCommand(MemberId);
    }

    public DeleteTeamMemberTestContext WithMemberExisting()
    {
        RepositoryMock
            .Setup(r => r.GetByIdAsync(MemberId))
            .ReturnsAsync(new TeamMemberEntity(
                MemberId, "Aleksa", "aleksa123", "aleksa.pele03@gmail.com",
                "hashed-password", 40, Role.User, MemberStatus.Active));

        return this;
    }

    public DeleteTeamMemberTestContext WithMemberNotFound()
    {
        RepositoryMock
            .Setup(r => r.GetByIdAsync(MemberId))
            .ReturnsAsync((TeamMemberEntity?)null);

        return this;
    }
}