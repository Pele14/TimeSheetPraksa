using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.TeamMemberRequests;
using TimeSheet.Application.TeamMember.Commands.CreateTeamMember;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;

namespace TimeSheet.Application.Tests.TeamMembers.Commands;



[TestFixture]
public class CreateTeamMemberHandlerTests
{
    private CreateTeamMemberTestContext context=null!;

    [SetUp]
    public void SetUp()
    {
            context=new CreateTeamMemberTestContext();
    }
    
    [Test]   
    public void Handle_EmailAlreadyExists_ThrowsConflictException()
    {
        context.WithEmailTaken();

        Assert.ThrowsAsync<ConflictException>(() =>
            context.Handler.Handle(context.CreateValidCommand(), CancellationToken.None));

        context.RepositoryMock.Verify(r => r.AddAsync(It.IsAny<TeamMemberEntity>()), Times.Never);
    }
    
    [Test]
    public async Task Handle_ValidCommand_AddsMemberAndReturnsDto()
    {
        context.WithEmailAvailable();

        var result = await context.Handler.Handle(context.CreateValidCommand(), CancellationToken.None);

        context.RepositoryMock.Verify(r => r.AddAsync(It.IsAny<TeamMemberEntity>()), Times.Once);
        Assert.That(result.Email, Is.EqualTo(CreateTeamMemberTestContext.Email));
        Assert.That(result.Status, Is.EqualTo(MemberStatus.Active));
    }

    [Test]
    public async Task Handle_ValidCommand_StoresHashedPassword()
    {
        context.WithEmailAvailable();

        await context.Handler.Handle(context.CreateValidCommand(), CancellationToken.None);

        context.RepositoryMock.Verify(r => r.AddAsync(
            It.Is<TeamMemberEntity>(m => m.PasswordHash == CreateTeamMemberTestContext.PasswordHash)),
            Times.Once);
    }
}
public class CreateTeamMemberTestContext
{
    public const string Email = "aleksa@gmail.com";
    public const string Password = "test123";
    public const string PasswordHash = "hashed-password";

    public Mock<ITeamMemberRepository> RepositoryMock { get; } = new();
    public Mock<IPasswordHasher> PasswordHasherMock { get; } = new();
    public CreateTeamMemberHandler Handler { get; }

    public CreateTeamMemberTestContext()
    {
        Handler = new CreateTeamMemberHandler(RepositoryMock.Object, PasswordHasherMock.Object);

        PasswordHasherMock
            .Setup(h => h.Hash(Password))
            .Returns(PasswordHash);
    }

    public CreateTeamMemberCommand CreateValidCommand()
    {
        return new CreateTeamMemberCommand(new CreateTeamMemberRequest
        {
            Name = "Aleksa",
            Username = "aleksa",
            Email = Email,
            Password = Password,
            HoursPerWeek = 40,
            Role = Role.Admin,
            Status=MemberStatus.Active
        });
    }

    public CreateTeamMemberTestContext WithEmailTaken()
    {
        RepositoryMock
            .Setup(r => r.ExistsByEmailAsync(Email))
            .ReturnsAsync(true);

        return this;
    }

    public CreateTeamMemberTestContext WithEmailAvailable()
    {
        RepositoryMock
            .Setup(r => r.ExistsByEmailAsync(Email))
            .ReturnsAsync(false);

        return this;
    }
}