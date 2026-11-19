
using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Auth;
using TimeSheet.Application.Auth.DTOs;
using TimeSheet.Application.Common;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.Tests.Auth;

[TestFixture]
public class LoginServiceTests
{
    private Mock<ITeamMemberRepository> repositoryMock = null!;
    private Mock<IPasswordHasher> passwordHasherMock = null!;

    private Mock<ITokenService> tokenServiceMock = null!;
    private AuthService authService = null!;

    [SetUp]
    public void SetUp()
    {
        repositoryMock = new Mock<ITeamMemberRepository>();
        passwordHasherMock = new Mock<IPasswordHasher>();
        tokenServiceMock = new Mock<ITokenService>();

        authService = new AuthService(
            tokenServiceMock.Object,

            passwordHasherMock.Object,
            repositoryMock.Object

        );
    }
    private static TeamMemberEntity CreateMember()
    {
        return new TeamMemberEntity
        (
            Guid.NewGuid(),
            "Aleksa",
            "aleksa123",
            "aleksa.pele03@gmail.com",
            "hashed",
            40,
            Role.Admin,
            MemberStatus.Active

        );
    }

    private static LoginDTO CreateLoginDto(string email = "aleksa.pele03@gmail.com", string password = "test123")
    {
        return new LoginDTO
        {
            Email = email,
            Password = password
        };
    }
    [Test]
    public async Task LoginAsync_Valid()
    {

        var member = CreateMember();
        repositoryMock
            .Setup(r => r.GetByEmailAsync(member.Email))
            .ReturnsAsync(member);

        passwordHasherMock
            .Setup(h => h.Verify("test123", member.PasswordHash))
            .Returns(true);
        tokenServiceMock
            .Setup(t => t.GenerateToken(member.Id, It.IsAny<string>()))
            .Returns("jwtfake");

        var result = await authService.LoginAsync(CreateLoginDto());

        Assert.That(result, Is.EqualTo("jwtfake"));

    }
    [Test]
    public void LoginAsync_EmailNotFound()
    {

        repositoryMock
            .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((TeamMemberEntity?)null);

        Assert.ThrowsAsync<UnauthorizedException>(() =>
            authService.LoginAsync(CreateLoginDto()));

        tokenServiceMock.Verify(
            t => t.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>()),
            Times.Never);
    }
    [Test]
    public void LoginAsync_WrongPassword()
    {

        var member = CreateMember();
        repositoryMock
           .Setup(r => r.GetByEmailAsync(member.Email));

        passwordHasherMock
                .Setup(h => h.Verify("pogresna", member.PasswordHash));

        Assert.ThrowsAsync<UnauthorizedException>(() =>
          authService.LoginAsync(CreateLoginDto(password: "pogresna")));

        tokenServiceMock.Verify(
            t => t.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>()),
            Times.Never);
    }
    public async Task LoginAsync_Normalized()
    {

        var member = CreateMember();
        repositoryMock
            .Setup(r => r.GetByEmailAsync(member.Email));

        passwordHasherMock
            .Setup(h => h.Verify("test123", member.PasswordHash));

        tokenServiceMock
            .Setup(t => t.GenerateToken(member.Id, It.IsAny<string>()))
            .Returns("jwtfake");

        var result = await authService.LoginAsync(CreateLoginDto(email: "AleksA.PeLE03@gmail.COM"));

        Assert.That(result, Is.EqualTo("jwtfake"));

    }


}
