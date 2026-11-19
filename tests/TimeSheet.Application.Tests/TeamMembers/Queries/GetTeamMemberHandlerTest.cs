using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.TeamMember.Queries.GetTeamMemberById;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.Tests.TeamMembers.Queries;

[TestFixture]
public class GetTeamMemberHandlerTests
{
    private GetTeamMemberTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new GetTeamMemberTestContext();
    }

    [Test]
    public void Handle_MemberNotFound_ThrowsNotFoundException()
    {
        context.WithMemberNotFound();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.CreateQuery(), CancellationToken.None));
    }

    [Test]
    public async Task Handle_MemberFound()
    {
        context.WithMemberExisting();

        var result = await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        var member = context.Member;
        Assert.That(result.Id, Is.EqualTo(member.Id));
        Assert.That(result.Name, Is.EqualTo(member.Name));
        Assert.That(result.Username, Is.EqualTo(member.Username));
        Assert.That(result.Email, Is.EqualTo(member.Email));
        Assert.That(result.HoursPerWeek, Is.EqualTo(member.HoursPerWeek));
        Assert.That(result.Role, Is.EqualTo(member.Role));
        Assert.That(result.Status, Is.EqualTo(member.Status));
    }
}

public class GetTeamMemberTestContext
{
    public Guid MemberId { get; } = Guid.NewGuid();
    public TeamMemberEntity Member { get; }

    public Mock<ITeamMemberRepository> RepositoryMock { get; } = new();
    public GetTeamMemberByIdHandler Handler { get; }

    public GetTeamMemberTestContext()
    {
        Handler = new GetTeamMemberByIdHandler(RepositoryMock.Object);

        Member = new TeamMemberEntity(
            MemberId, "Aleksa", "aleksa123", "aleksa.pele03@gmail.com",
            "hashed-password", 40, Role.User, MemberStatus.Active);
    }

    public GetTeamMemberByIdQuery CreateQuery()
    {
        return new GetTeamMemberByIdQuery(MemberId);
    }

    public GetTeamMemberTestContext WithMemberExisting()
    {
        RepositoryMock
            .Setup(r => r.GetByIdAsync(MemberId))
            .ReturnsAsync(Member);

        return this;
    }

    public GetTeamMemberTestContext WithMemberNotFound()
    {
        RepositoryMock
            .Setup(r => r.GetByIdAsync(MemberId))
            .ReturnsAsync((TeamMemberEntity?)null);

        return this;
    }
}