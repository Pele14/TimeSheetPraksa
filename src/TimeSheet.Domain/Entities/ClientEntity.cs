namespace Timesheet.Domain.Entities;

public class ClientEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }

    public string Zip { get; set; }

    public Guid CountryEntityId { get; set; }

    public ClientEntity(Guid id, string name, string addres, string city, string zip, Guid id1)
    {
        Id = id;
        Name = name;
        Address = addres;
        City = city;
        Zip = zip;
        CountryEntityId = id1;
    }

}
