using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.TeamMember.Commands.UpdateTeamMember;
using TimeSheet.Application.TeamMember.DTOs;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.Tests.TeamMembers.Commands;


[TestFixture]
public class UpdateTeamMemberHandlerTests
{
    private Mock<ITeamMemberRepository> _repositoryMock = null!;
    private UpdateTeamMemberHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<ITeamMemberRepository>();
        _handler = new UpdateTeamMemberHandler(_repositoryMock.Object);
    }

    private static TeamMemberEntity CreateExistingMember(Guid id)
    {
        return new TeamMemberEntity(
            id, "Aleksa", "aleksa123", "aleksa.pele03@gmail.com",
            "hashed-password", 40, Role.User, MemberStatus.Active);
    }

    private static UpdateTeamMemberCommand CreateUpdateCommand(Guid id, string email)
    {
        return new UpdateTeamMemberCommand(id, new UpdateTeamMemberDto
        {
            Name = "Marko",
            Username = "marko123",
            Email = email,
            HoursPerWeek = 35,
            Role = Role.Admin,
            Status = MemberStatus.Inactive
        });
    }

    [Test]
    public void Handle_MemberNotFound_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((TeamMemberEntity?)null);

        var command = CreateUpdateCommand(id, "aleksa.pele03@gmail.com");

        Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TeamMemberEntity>()), Times.Never);
    }
    [Test]
    public void Handle_NewEmailTakenByAnotherMember_ThrowsConflictException()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(CreateExistingMember(id));

        _repositoryMock
            .Setup(r => r.ExistsByEmailAsync("marko@gmail.com"))
            .ReturnsAsync(true);

        var command = CreateUpdateCommand(id, "marko@gmail.com");

        Assert.ThrowsAsync<ConflictException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TeamMemberEntity>()), Times.Never);
    }
    [Test]
    public async Task Handle_SameEmail_DoesNotCheckForDuplicate()
    {

        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(CreateExistingMember(id));

        var command = CreateUpdateCommand(id, "aleksa.pele03@gmail.com");

        await _handler.Handle(command, CancellationToken.None);


        _repositoryMock.Verify(r => r.ExistsByEmailAsync(It.IsAny<string>()), Times.Never);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TeamMemberEntity>()), Times.Once);
    }
    [Test]
    public async Task Handle_ValidCommand_UpdatesMemberWithNewValues()
    {

        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(CreateExistingMember(id));

        _repositoryMock
            .Setup(r => r.ExistsByEmailAsync("marko@gmail.com"))
            .ReturnsAsync(false);

        var command = CreateUpdateCommand(id, "marko@gmail.com");

        await _handler.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(r => r.UpdateAsync(It.Is<TeamMemberEntity>(m =>
            m.Id == id &&
            m.Name == "Marko" &&
            m.Username == "marko123" &&
            m.Email == "marko@gmail.com" &&
            m.HoursPerWeek == 35 &&
            m.Role == Role.Admin &&
            m.Status == MemberStatus.Inactive &&
            m.PasswordHash == "hashed-password")),
            Times.Once);
    }

}