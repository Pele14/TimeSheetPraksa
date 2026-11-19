using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.TeamMember.Queries.GetTeamMembers;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.Tests.TeamMembers.Queries;

[TestFixture]
public class GetAllTeamMemberHandlerTests
{
    private GetAllTeamMemberTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new GetAllTeamMemberTestContext();
    }

    [Test]
    public async Task Handle_MembersExist()
    {
        context.WithMembers();

        var result = await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Id, Is.EqualTo(context.Members[0].Id));
        Assert.That(result[1].Id, Is.EqualTo(context.Members[1].Id));
        Assert.That(result[0].Name, Is.EqualTo(context.Members[0].Name));
        Assert.That(result[1].Name, Is.EqualTo(context.Members[1].Name));
    }

    [Test]
    public async Task Handle_NoMembers()
    {
        context.WithNoMembers();

        var result = await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }
}

public class GetAllTeamMemberTestContext
{
    public List<TeamMemberEntity> Members { get; } = new()
    {
        CreateMember("Aleksa", "aleksa.pele03@gmail.com"),
        CreateMember("Marko", "marko@gmail.com")
    };

    public Mock<ITeamMemberRepository> RepositoryMock { get; } = new();
    public GetTeamMembersHandler Handler { get; }

    public GetAllTeamMemberTestContext()
    {
        Handler = new GetTeamMembersHandler(RepositoryMock.Object);
    }

    public GetTeamMembersQuery CreateQuery()
    {
        return new GetTeamMembersQuery();
    }

    public GetAllTeamMemberTestContext WithMembers()
    {
        RepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(Members);

        return this;
    }

    public GetAllTeamMemberTestContext WithNoMembers()
    {
        RepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<TeamMemberEntity>());

        return this;
    }

    private static TeamMemberEntity CreateMember(string name, string email)
    {
        return new TeamMemberEntity(
            Guid.NewGuid(), name, name.ToLower(), email,
            "hashed-password", 40, Role.User, MemberStatus.Active);
    }
}