
namespace Timesheet.Domain.Entities;

   public class TimeEntryEntity
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public double Time { get; set; }

        public double? Overtime { get; set; }

        public string? Description { get; set; }

        public Guid CategoryId { get; set; }


        public Guid ProjectId { get; set; }


        public Guid TeamMemberId { get; set; }

    }
