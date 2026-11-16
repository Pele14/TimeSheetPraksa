namespace Timesheet.Domain.Entities;

    public class CountryEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public required string Code { get; set; }

        public ICollection<ClientEntity> Clients { get; set; } = new List<ClientEntity>();
    }
