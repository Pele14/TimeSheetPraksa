using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.TeamMember.Commands.DeleteTeamMember;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.Tests.TeamMembers.Commands;



[TestFixture]
public class DeleteTeamMemberHandlerTests
{
    private Mock<ITeamMemberRepository> _repositoryMock = null!;
    private DeleteTeamMemberHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<ITeamMemberRepository>();
        _handler = new DeleteTeamMemberHandler(_repositoryMock.Object);
    }

    private static TeamMemberEntity CreateExistingMember(Guid id)
    {
        return new TeamMemberEntity(
            id, "Aleksa", "aleksa123", "aleksa.pele03@gmail.com",
            "hashed-password", 40, Role.User, MemberStatus.Active);
    }

    [Test]
    public void Handle_MemberNotFound()
    {
        var id = Guid.NewGuid();
        _repositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((TeamMemberEntity?)null);
        var command = new DeleteTeamMemberCommand(id);

        Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );
        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);



    }

    [Test]
    public async Task Handle_MemberFound()
    {
        var id = Guid.NewGuid();
        _repositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(CreateExistingMember(id));
        var command = new DeleteTeamMemberCommand(id);


        await _handler.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);



    }

}