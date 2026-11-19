using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Auth;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.AuthRequests;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;

namespace TimeSheet.Application.Tests.Auth;

[TestFixture]
public class LoginServiceTests
{
    private LoginServiceTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new LoginServiceTestContext();
    }

    [Test]
    public async Task LoginAsync_Valid()
    {
        context
            .WithMemberExisting()
            .WithCorrectPassword();

        var result = await context.AuthService.LoginAsync(context.CreateLoginRequest());

        Assert.That(result.Token, Is.EqualTo(LoginServiceTestContext.Token));
    }

    [Test]
    public void LoginAsync_EmailNotFound()
    {
        context.WithMemberNotFound();

        Assert.ThrowsAsync<UnauthorizedException>(() =>
            context.AuthService.LoginAsync(context.CreateLoginRequest()));

        context.VerifyTokenNotGenerated();
    }

    [Test]
    public void LoginAsync_WrongPassword()
    {
        context
            .WithMemberExisting()
            .WithWrongPassword();

        Assert.ThrowsAsync<UnauthorizedException>(() =>
            context.AuthService.LoginAsync(
                context.CreateLoginRequest(password: LoginServiceTestContext.WrongPassword)));

        context.VerifyTokenNotGenerated();
    }

    [Test]
    public async Task LoginAsync_Normalized()
    {
        context
            .WithMemberExisting()
            .WithCorrectPassword();

        var result = await context.AuthService.LoginAsync(
            context.CreateLoginRequest(email: "  AleksA.PeLE03@gmail.COM  "));

        Assert.That(result.Token, Is.EqualTo(LoginServiceTestContext.Token));
        context.RepositoryMock.Verify(r => r.GetByEmailAsync(LoginServiceTestContext.Email), Times.Once);
    }
}

public class LoginServiceTestContext
{
    public const string Email = "aleksa.pele03@gmail.com";
    public const string Password = "test123";
    public const string WrongPassword = "pogresna";
    public const string PasswordHash = "hashed";
    public const string Token = "jwtfake";

    public TeamMemberEntity Member { get; }

    public Mock<ITeamMemberRepository> RepositoryMock { get; } = new();
    public Mock<IPasswordHasher> PasswordHasherMock { get; } = new();
    public Mock<ITokenService> TokenServiceMock { get; } = new();
    public AuthService AuthService { get; }

    public LoginServiceTestContext()
    {
        AuthService = new AuthService(
            TokenServiceMock.Object,
            PasswordHasherMock.Object,
            RepositoryMock.Object);

        Member = new TeamMemberEntity(
            Guid.NewGuid(), "Aleksa", "aleksa123", Email,
            PasswordHash, 40, Role.Admin, MemberStatus.Active);

        TokenServiceMock
            .Setup(t => t.GenerateToken(Member.Id, It.IsAny<string>()))
            .Returns(Token);
    }

    public LoginRequest CreateLoginRequest(string email = Email, string password = Password)
    {
        return new LoginRequest
        {
            Email = email,
            Password = password
        };
    }

    public LoginServiceTestContext WithMemberExisting()
    {
        RepositoryMock
            .Setup(r => r.GetByEmailAsync(Email))
            .ReturnsAsync(Member);

        return this;
    }

    public LoginServiceTestContext WithMemberNotFound()
    {
        RepositoryMock
            .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((TeamMemberEntity?)null);

        return this;
    }

    public LoginServiceTestContext WithCorrectPassword()
    {
        PasswordHasherMock
            .Setup(h => h.Verify(Password, PasswordHash))
            .Returns(true);

        return this;
    }

    public LoginServiceTestContext WithWrongPassword()
    {
        PasswordHasherMock
            .Setup(h => h.Verify(WrongPassword, PasswordHash))
            .Returns(false);

        return this;
    }

    public void VerifyTokenNotGenerated()
    {
        TokenServiceMock.Verify(
            t => t.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>()),
            Times.Never);
    }
}