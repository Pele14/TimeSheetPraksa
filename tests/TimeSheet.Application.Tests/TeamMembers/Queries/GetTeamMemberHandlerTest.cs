using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;

using TimeSheet.Application.TeamMember.Queries.GetTeamMemberById;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.Tests.TeamMembers.Commands.Queries;


[TestFixture]
public class GetTeamMemberHandlerTests
{
    private Mock<ITeamMemberRepository> _repositoryMock = null!;
    private GetTeamMemberByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<ITeamMemberRepository>();
        _handler = new GetTeamMemberByIdHandler(_repositoryMock.Object);
    }

    private static TeamMemberEntity CreateExistingMember(Guid id)
    {
        return new TeamMemberEntity(
            id, "Aleksa", "aleksa123", "aleksa.pele03@gmail.com",
            "hashed-password", 40, Role.User, MemberStatus.Active);
    }

    [Test]
    public void Handle_MemberNotFound_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((TeamMemberEntity?)null);

        var query = new GetTeamMemberByIdQuery(id);

        Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));
    }
    [Test]
    public async Task Handle_MemberFound()
    {
        var id = Guid.NewGuid();
        var entity = CreateExistingMember(id);
        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(entity);

        var query = new GetTeamMemberByIdQuery(id);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.That(result.Id, Is.EqualTo(id));
        Assert.That(result.Name, Is.EqualTo(entity.Name));
        Assert.That(result.Username, Is.EqualTo(entity.Username));
        Assert.That(result.Email, Is.EqualTo(entity.Email));
        Assert.That(result.HoursPerWeek, Is.EqualTo(entity.HoursPerWeek));
        Assert.That(result.Role, Is.EqualTo(entity.Role));
        Assert.That(result.Status, Is.EqualTo(entity.Status));
    }


}
