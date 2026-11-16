namespace Timesheet.Domain.Entities;

    public class TeamMemberEntity
    {
        public Guid Id { get; set; } 

        public required string Name { get; set; }

        public required string Username { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public double HoursPerWeek { get; set; }

        public Role Role { get; set; }

        public MemberStatus Status { get; set; }

        public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();

        public ICollection<TimeEntryEntity> TimeEntries { get; set; } = new List<TimeEntryEntity>();
}
