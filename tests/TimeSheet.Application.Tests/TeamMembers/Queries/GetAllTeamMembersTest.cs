using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;

using TimeSheet.Application.TeamMember.Queries.GetTeamMemberById;
using TimeSheet.Application.TeamMember.Queries.GetTeamMembers;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.Tests.TeamMembers.Commands.Queries;


[TestFixture]
public class GetAllTeamMemberHandlerTests
{
    private Mock<ITeamMemberRepository> _repositoryMock = null!;
    private GetTeamMembersHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<ITeamMemberRepository>();
        _handler = new GetTeamMembersHandler(_repositoryMock.Object);
    }

    private static TeamMemberEntity CreateMember(string name, string email)
    {
        return new TeamMemberEntity(
            Guid.NewGuid(), name, name.ToLower(), email,
            "hashed-password", 40, Role.User, MemberStatus.Active);
    }

    [Test]
    public async Task Handle_MembersExist()
    {
        var members = new List<TeamMemberEntity>
    {
        CreateMember("Aleksa", "aleksa.pele03@gmail.com"),
        CreateMember("Marko", "marko@gmail.com")

    };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(members);


        var result = await _handler.Handle(new GetTeamMembersQuery(), CancellationToken.None);

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Id, Is.EqualTo(members[0].Id));
        Assert.That(result[1].Id, Is.EqualTo(members[1].Id));
        Assert.That(result[0].Name, Is.EqualTo(members[0].Name));
        Assert.That(result[1].Name, Is.EqualTo(members[1].Name));
    }
    [Test]
    public async Task Handle_noMembers()
    {

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<TeamMemberEntity>());


        var result = await _handler.Handle(new GetTeamMembersQuery(), CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }


}
