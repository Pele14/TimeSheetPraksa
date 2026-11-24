using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.TeamMemberRequests;
using TimeSheet.Application.TeamMember.Commands.UpdateTeamMember;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.Tests.TeamMembers.Commands;

[TestFixture]
public class UpdateTeamMemberHandlerTests
{
    private UpdateTeamMemberTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new UpdateTeamMemberTestContext();
    }

    [Test]
    public void Handle_MemberNotFound_ThrowsNotFoundException()
    {
        context.WithMemberNotFound();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(
                context.CreateCommand(UpdateTeamMemberTestContext.ExistingEmail),
                CancellationToken.None));

        context.RepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TeamMemberModel>()), Times.Never);
    }

    [Test]
    public void Handle_NewEmailTakenByAnotherMember_ThrowsConflictException()
    {
        context
            .WithMemberExisting()
            .WithEmailTaken(UpdateTeamMemberTestContext.NewEmail);

        Assert.ThrowsAsync<ConflictException>(() =>
            context.Handler.Handle(
                context.CreateCommand(UpdateTeamMemberTestContext.NewEmail),
                CancellationToken.None));

        context.RepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TeamMemberModel>()), Times.Never);
    }

    [Test]
    public async Task Handle_SameEmail_DoesNotCheckForDuplicate()
    {
        context.WithMemberExisting();

        await context.Handler.Handle(
            context.CreateCommand(UpdateTeamMemberTestContext.ExistingEmail),
            CancellationToken.None);

        context.RepositoryMock.Verify(r => r.ExistsByEmailAsync(It.IsAny<string>()), Times.Never);
        context.RepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TeamMemberModel>()), Times.Once);
    }

    [Test]
    public async Task Handle_ValidCommand_UpdatesMemberWithNewValues()
    {
        context
            .WithMemberExisting()
            .WithEmailAvailable(UpdateTeamMemberTestContext.NewEmail);

        await context.Handler.Handle(
            context.CreateCommand(UpdateTeamMemberTestContext.NewEmail),
            CancellationToken.None);

        context.RepositoryMock.Verify(r => r.UpdateAsync(It.Is<TeamMemberModel>(m =>
            m.Id == context.MemberId &&
            m.Name == "Marko" &&
            m.Username == "marko123" &&
            m.Email == UpdateTeamMemberTestContext.NewEmail &&
            m.HoursPerWeek == 35 &&
            m.Role == Role.Admin &&
            m.Status == MemberStatus.Inactive &&
            m.PasswordHash == UpdateTeamMemberTestContext.PasswordHash)),
            Times.Once);
    }
}

public class UpdateTeamMemberTestContext
{
    public const string ExistingEmail = "aleksa.pele03@gmail.com";
    public const string NewEmail = "marko@gmail.com";
    public const string PasswordHash = "hashed-password";

    public Guid MemberId { get; } = Guid.NewGuid();

    public Mock<ITeamMemberRepository> RepositoryMock { get; } = new();
    public UpdateTeamMemberHandler Handler { get; }

    public UpdateTeamMemberTestContext()
    {
        Handler = new UpdateTeamMemberHandler(RepositoryMock.Object);
    }

    public UpdateTeamMemberCommand CreateCommand(string email)
    {
        return new UpdateTeamMemberCommand(MemberId, new UpdateTeamMemberRequest
        {
            Name = "Marko",
            Username = "marko123",
            Email = email,
            HoursPerWeek = 35,
            Role = Role.Admin,
            Status = MemberStatus.Inactive
        });
    }

    public UpdateTeamMemberTestContext WithMemberExisting()
    {
        RepositoryMock
            .Setup(r => r.GetByIdAsync(MemberId))
            .ReturnsAsync(new TeamMemberModel(
                MemberId, "Aleksa", "aleksa123", ExistingEmail,
                PasswordHash, 40, Role.User, MemberStatus.Active));

        return this;
    }

    public UpdateTeamMemberTestContext WithMemberNotFound()
    {
        RepositoryMock
            .Setup(r => r.GetByIdAsync(MemberId))
            .ReturnsAsync((TeamMemberModel?)null);

        return this;
    }

    public UpdateTeamMemberTestContext WithEmailTaken(string email)
    {
        RepositoryMock
            .Setup(r => r.ExistsByEmailAsync(email))
            .ReturnsAsync(true);

        return this;
    }

    public UpdateTeamMemberTestContext WithEmailAvailable(string email)
    {
        RepositoryMock
            .Setup(r => r.ExistsByEmailAsync(email))
            .ReturnsAsync(false);

        return this;
    }
}