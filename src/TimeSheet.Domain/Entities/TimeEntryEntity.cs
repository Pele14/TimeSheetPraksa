
namespace Timesheet.Domain.Entities;

   public class TimeEntryEntity
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public double Time { get; set; }

        public double Overtime { get; set; }

        public string? Description { get; set; }

        public Guid CategoryId { get; set; }


        public Guid ProjectId { get; set; }


        public Guid TeamMemberId { get; set; }

        public TimeEntryEntity(Guid id, DateTime date, double time, double overtime, string? description, Guid id1, Guid id2, Guid id3)
    {
        Id=id;
        Date=date;
        Time=time;
        Overtime=overtime;
        Description=description;
        CategoryId=id1;
        ProjectId=id2;
        TeamMemberId=id3;

    }

    }
