

using TimeSheet.Domain.Exceptions;
using Timesheet.Domain.Entities;

namespace TimeSheet.Domain.Tests.Entities;


[TestFixture]
public class TeamMemberEnityTests
{
    private static TeamMemberEntity CreateValidMember(Guid id)
    {
        return new TeamMemberEntity(
             id,
            "Aleksa",
            "aleksa123",
            "aleksa.pele03@gmail.com",
            "1234",
            40,
            Role.Admin,
            MemberStatus.Active);

    }

    [Test]
    public void Constructor_ValidData_Sets_Properties()
    {
        var id = Guid.NewGuid();
        var member = CreateValidMember(id);
        Assert.That(member.Id, Is.EqualTo(id));
        Assert.That(member.Name, Is.EqualTo("Aleksa"));
        Assert.That(member.Username, Is.EqualTo("aleksa123"));
        Assert.That(member.Email, Is.EqualTo("aleksa.pele03@gmail.com"));
        Assert.That(member.HoursPerWeek, Is.EqualTo(40));
        Assert.That(member.Role, Is.EqualTo(Role.Admin));
        Assert.That(member.Status, Is.EqualTo(MemberStatus.Active));
    }

    [TestCase("")]
    [TestCase(" ")]
    public void Constructor_InvalidName(string name)
    {
        Assert.Throws<DomainException>(() =>
            new TeamMemberEntity(Guid.NewGuid(), name, "aleksa123", "aleksa.pele03@gmail.com"
            , "123", 40, Role.Admin, MemberStatus.Active
             )
        );

    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase("bademail")]
    public void Constructor_InvalidEmail(string email)
    {
        Assert.Throws<DomainException>(() =>
            new TeamMemberEntity(Guid.NewGuid(), "aleksa", "aleksa123", email
            , "123", 40, Role.Admin, MemberStatus.Active
             )
        );

    }

    [TestCase("")]
    [TestCase(" ")]
    public void Constructor_InvalidUsername(string username)
    {
        Assert.Throws<DomainException>(() =>
            new TeamMemberEntity(Guid.NewGuid(), "aleksa", username, "aleksa.pele03@gmail.com"
            , "123", 40, Role.Admin, MemberStatus.Active
             )
        );

    }
    [TestCase(0)]
    [TestCase(-2)]
    public void Constructor_InvalidHours(double hours)
    {
        Assert.Throws<DomainException>(() =>
            new TeamMemberEntity(Guid.NewGuid(), "aleksa", "aleksa", "aleksa.pele03@gmail.com"
            , "123", hours, Role.Admin, MemberStatus.Active
             )
        );

    }
    [Test]
    public void Constructor_InvalidRole()
    {
        Assert.Throws<DomainException>(() =>
            new TeamMemberEntity(
                Guid.NewGuid(), "Aleksa", "aleksa123", "aleksa.pele@gmail.com",
                "123", 40, (Role)99, MemberStatus.Active));
    }

    [Test]
    public void Constructor_InvalidStatusn()
    {
        Assert.Throws<DomainException>(() =>
            new TeamMemberEntity(
                Guid.NewGuid(), "Aleksa", "aleksa123", "aleksa.pele03@gmail.com",
                "123", 40, Role.Admin, (MemberStatus)99));
    }
    [Test]
    public void Constructor_EmailWithUppercaseAndSpaces_NormalizesEmail()
    {
        var member = new TeamMemberEntity(
            Guid.NewGuid(), "Aleksa", "aleksa", "ALeksa.PELE@gmail.COM  ",
            "123", 40, Role.Admin, MemberStatus.Active);

        Assert.That(member.Email, Is.EqualTo("aleksa.pele@gmail.com"));
    }

    [Test]
    public void Update_ValidData_ChangesProperties()
    {

        var member = CreateValidMember(Guid.NewGuid());
        member.Update("Petar", "petar", "petar@gmail.com", 35, Role.Admin, MemberStatus.Inactive);


        Assert.That(member.Name, Is.EqualTo("Petar"));
        Assert.That(member.Username, Is.EqualTo("petar"));
        Assert.That(member.Email, Is.EqualTo("petar@gmail.com"));
        Assert.That(member.HoursPerWeek, Is.EqualTo(35));
        Assert.That(member.Role, Is.EqualTo(Role.Admin));
        Assert.That(member.Status, Is.EqualTo(MemberStatus.Inactive));

    }
    [TestCase("")]
    [TestCase("   ")]
    public void Update_InvalidName(string name)
    {
        var member = CreateValidMember(Guid.NewGuid());

        Assert.Throws<DomainException>(() =>
            member.Update(name, "petar", "petar@gmail.com", 35, Role.Admin, MemberStatus.Active));
    }
    [TestCase("")]
    [TestCase("   ")]
    public void Update_InvalidUsername(string username)
    {
        var member = CreateValidMember(Guid.NewGuid());

        Assert.Throws<DomainException>(() =>
            member.Update("Petar", username, "petar@gmail.com", 35, Role.Admin, MemberStatus.Active));
    }
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("notemail")]
    public void Update_InvalidEmail_(string email)
    {
        var member = CreateValidMember(Guid.NewGuid());

        Assert.Throws<DomainException>(() =>
            member.Update("Petar", "petar", email, 35, Role.Admin, MemberStatus.Active));
    }

    [TestCase(0)]
    [TestCase(-5)]
    public void Update_InvalidHoursPerWeek(double hours)
    {
        var member = CreateValidMember(Guid.NewGuid());

        Assert.Throws<DomainException>(() =>
            member.Update("Petar", "petar", "petar@gmail.com", hours, Role.Admin, MemberStatus.Active));
    }
    [Test]
    public void Update_InvalidData_DoesNotChangeProperties()
    {
        var member = CreateValidMember(Guid.NewGuid());
        var originalName = member.Name;
        var originalUsername = member.Username;
        var originalEmail = member.Email;
        var originalHours = member.HoursPerWeek;
        var originalRole = member.Role;

        Assert.Throws<DomainException>(() =>
            member.Update("", "petar", "petar@gmail.com", 35, Role.Admin, MemberStatus.Inactive));

        Assert.That(member.Name, Is.EqualTo(originalName));
        Assert.That(member.Username, Is.EqualTo(originalUsername));
        Assert.That(member.Email, Is.EqualTo(originalEmail));
        Assert.That(member.HoursPerWeek, Is.EqualTo(originalHours));
        Assert.That(member.Role, Is.EqualTo(originalRole));
    }
    [Test]
    public void Update_ValidData_DoesNotChangePasswordHash()
    {
        // Arrange
        var member = CreateValidMember(Guid.NewGuid());
        var originalHash = member.PasswordHash;

        // Act
        member.Update("Petar", "petar", "petar@gmail.com", 35, Role.Admin, MemberStatus.Inactive);

        // Assert
        Assert.That(member.PasswordHash, Is.EqualTo(originalHash));
    }

    [Test]
    public void Update_EmailWithUppercaseAndSpaces_NormalizesEmail()
    {
        var member = CreateValidMember(Guid.NewGuid());

        member.Update("Petar", "petar", "  Petar@Gmail.COM  ", 35, Role.Admin, MemberStatus.Active);

        Assert.That(member.Email, Is.EqualTo("petar@gmail.com"));
    }

}