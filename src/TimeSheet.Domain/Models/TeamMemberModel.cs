using TimeSheet.Domain.Exceptions;

namespace Timesheet.Domain.Entities;

public class TeamMemberModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public double HoursPerWeek { get; set; }
    public Role Role { get; set; }
    public MemberStatus Status { get; set; }

    public TeamMemberModel(Guid id, string name, string username, string email, string password, double hours, Role role, MemberStatus status)
    {
        Validate(name, username, email, hours, role, status);
        Id = id;
        Name = name;
        Username = username;
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = password;
        HoursPerWeek = hours;
        Role = role;
        Status = status;

    }
    public void Update(string name, string username, string email, double hours, Role role, MemberStatus status)
    {
        var normalizedEmail = email?.Trim().ToLowerInvariant() ?? string.Empty;
        Validate(name, username, normalizedEmail, hours, role, status);
        Name = name;
        Username = username;
        Email = normalizedEmail;
        HoursPerWeek = hours;
        Role = role;
        Status = status;
    }

    public static void Validate(string name, string username, string email, double hours, Role role, MemberStatus status)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is requiered");
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username is required");

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new DomainException("Bad email");

        if (hours <= 0)
            throw new DomainException("HoursPerWeek must be greater then 0");

        if (!Enum.IsDefined(role))
            throw new DomainException("Role doesnt exist");

        if (!Enum.IsDefined(status))
            throw new DomainException("Status doesnt exist");
    }

}

