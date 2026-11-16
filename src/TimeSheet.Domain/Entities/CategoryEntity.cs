

namespace Timesheet.Domain.Entities;

  public class CategoryEntity 
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public ICollection<TimeEntryEntity> TimeEntries { get; set; } = new List<TimeEntryEntity>();
    }
