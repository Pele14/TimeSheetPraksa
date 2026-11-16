namespace Timesheet.Domain.Entities;


    public class ProjectEntity
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public ProjectStatus ProjectStatus { get; set; }

        public Guid ClientId { get; set; }

        public Guid TeamMemberId { get; set; }

        public ICollection<TimeEntryEntity> TimeEntries { get; set; } = new List<TimeEntryEntity>();

    }
