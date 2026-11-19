using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.TeamMember.Commands.CreateTeamMember;
using TimeSheet.Application.TeamMember.DTOs;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.Tests.TeamMembers.Commands;



[TestFixture]
public class CreateTeamMemberHandlerTests
{
    private Mock<ITeamMemberRepository> repositoryMock = null!;
    private Mock<IPasswordHasher> passwordHasherMock = null!;
    private CreateTeamMemberHandler handler = null!;

    [SetUp]
    public void SetUp()
    {
        repositoryMock = new Mock<ITeamMemberRepository>();
        passwordHasherMock = new Mock<IPasswordHasher>();
        handler = new CreateTeamMemberHandler(repositoryMock.Object, passwordHasherMock.Object);
    }
    private static CreateTeamMemberCommand CreateValidCommand()
    {
        return new CreateTeamMemberCommand(new CreateTeamMemberDTO
        {
            Name = "Aleksa",
            Username = "aleksa123",
            Email = "aleksa.pele03@gmail.com",
            PasswordHash = "test123",
            HoursPerWeek = 40,
            Role = Role.Admin,
            Status = MemberStatus.Active


        });

    }
    [Test]
    public void Handle_EmailAlreadyExists()
    {
        repositoryMock
            .Setup(r => r.ExistsByEmailAsync("aleksa.pele03@gmail.com"))// simuliraj da postoji ovaj mail u repo
            .ReturnsAsync(true);

        var command = CreateValidCommand();

        Assert.ThrowsAsync<ConflictException>(() =>
            handler.Handle(command, CancellationToken.None)

        );
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<TeamMemberEntity>()), Times.Never);
        //obezbedjuje da addAsync nije bacen

    }
    [Test]
    public async Task Handle_ValidCommand_AddsMemberAndReturnsDto()
    {
        // Arrange
        repositoryMock
            .Setup(r => r.ExistsByEmailAsync("aleksa.pele03@gmail.com"))
            .ReturnsAsync(false);

        passwordHasherMock
            .Setup(h => h.Hash("test123"))
            .Returns("hashed-password");

        var command = CreateValidCommand();

        var result = await handler.Handle(command, CancellationToken.None);


        repositoryMock.Verify(r => r.AddAsync(It.IsAny<TeamMemberEntity>()), Times.Once);

        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Name, Is.EqualTo("Aleksa"));
        Assert.That(result.Username, Is.EqualTo("aleksa123"));
        Assert.That(result.Email, Is.EqualTo("aleksa.pele03@gmail.com"));
        Assert.That(result.HoursPerWeek, Is.EqualTo(40));
        Assert.That(result.Role, Is.EqualTo(Role.Admin));
        Assert.That(result.Status, Is.EqualTo(MemberStatus.Active));
    }

    [Test]
    public async Task Handle_ValidCommand_StoresHashedPassword()
    {
        repositoryMock
            .Setup(r => r.ExistsByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        passwordHasherMock
            .Setup(h => h.Hash("test123"))
            .Returns("hashed-password");


        await handler.Handle(CreateValidCommand(), CancellationToken.None);

        passwordHasherMock.Verify(h => h.Hash("test123"), Times.Once);

        repositoryMock.Verify(r => r.AddAsync(
            It.Is<TeamMemberEntity>(m => m.PasswordHash == "hashed-password")),
            Times.Once);
    }
}