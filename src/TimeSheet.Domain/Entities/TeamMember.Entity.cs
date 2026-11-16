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
}
