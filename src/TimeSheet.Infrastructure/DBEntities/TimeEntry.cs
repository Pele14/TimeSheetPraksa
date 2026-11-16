
using TimeSheet.Infrastructure.Persistence;

namespace Timesheet.Infrastructure.DBEntities;

   public class TimeEntry: ISoftDelitable
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public double Time { get; set; }

        public double? Overtime { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public Guid ProjectId { get; set; }

        public Project Project { get; set; } = null!;

        public Guid TeamMemberId { get; set; }

        public TeamMember TeamMember { get; set; } = null!;
    }
