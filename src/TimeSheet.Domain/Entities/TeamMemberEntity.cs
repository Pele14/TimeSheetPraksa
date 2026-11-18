namespace Timesheet.Domain.Entities;

    public class TeamMemberEntity
    {
        public Guid Id { get; set; } 

        public string Name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public double HoursPerWeek { get; set; }

        public Role Role { get; set; }

        public MemberStatus Status { get; set; }

       public TeamMemberEntity(Guid id, string name, string username, string email, string password, double hours, Role role, MemberStatus status)
    {
        Id=id;
        Name=name;
        Username=username;
        Email=email;
        PasswordHash=password;
        HoursPerWeek=hours;
        Role=role;
        Status=status;

    }
    public void Update(string name, string username, string email, double hours, Role role, MemberStatus status)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        if(string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required");
        if(string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        if (HoursPerWeek < 0)
            throw new ArgumentException("HoursPerWeek cant be negative");

        Name=name;
        Username=username;
        Email=email.Trim().ToLowerInvariant();
        HoursPerWeek=hours;
        Role=role;
        Status=status;
    } 
}
