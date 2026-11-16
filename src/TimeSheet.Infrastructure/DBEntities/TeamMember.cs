using TimeSheet.Infrastructure.Persistence;

namespace Timesheet.Infrastructure.DBEntities;

    public class TeamMember: ISoftDelitable, IAuditable
    {
        public Guid Id { get; set; } 

        public required string Name { get; set; }

        public required string Username { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public double HoursPerWeek { get; set; }

        public Role Role { get; set; }

        public MemberStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();

        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
}
