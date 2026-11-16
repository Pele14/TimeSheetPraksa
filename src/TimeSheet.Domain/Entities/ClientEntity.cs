namespace Timesheet.Domain.Entities;

    public class ClientEntity
    {
        public Guid Id{ get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }

        public required string Zip{ get; set;    }

        public Guid CountryEntityId { get; set; }

        public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
    }
