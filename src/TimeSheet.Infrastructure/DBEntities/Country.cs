using TimeSheet.Infrastructure.Persistence;

namespace Timesheet.Infrastructure.DBEntities;

    public class Country: ISoftDelitable, IAuditable

    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public required string Code { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }
