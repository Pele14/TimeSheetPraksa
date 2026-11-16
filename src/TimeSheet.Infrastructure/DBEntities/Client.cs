using TimeSheet.Infrastructure.Persistence;

namespace Timesheet.Infrastructure.DBEntities;

    public class Client: ISoftDelitable
    {
        public Guid Id{ get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }

        public required string Zip{ get; set;    }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public Guid CountryId { get; set; }

        public Country Country { get; set; }=null!;

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
