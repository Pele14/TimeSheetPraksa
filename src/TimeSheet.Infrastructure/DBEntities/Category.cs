

using TimeSheet.Infrastructure.Persistence;

namespace Timesheet.Infrastructure.DBEntities;


  public class Category: ISoftDelitable
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public DateTime CreatedAt { get; set; } 

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    }
