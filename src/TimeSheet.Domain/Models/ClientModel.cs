using TimeSheet.Domain.Enums;
using TimeSheet.Domain.Exceptions;

namespace Timesheet.Domain.Entities;

public class ClientModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }

    public string Zip { get; set; }

    public Country Country { get; set; }

    public ClientModel(Guid id, string name, string address, string city, string zip, Country country)
    {
        Validate(name, address, city, zip, country);
        Id = id;
        Name = name;
        Address = address;
        City = city;
        Zip = zip;
        Country=country;
    }
    public void Update(string name, string address, string city, string zip, Country country)
    {
        Validate(name, address, city, zip, country);
        Name = name;
        Address = address;
        City=city;
        Zip=zip;
        Country=country;
    }

    public static void Validate(string name, string address, string city, string zip, Country country)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name is requiered");
        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("address is required");
        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException("City is required");
        if (string.IsNullOrWhiteSpace(zip))
            throw new DomainException("Zip is required");
        if (!Enum.IsDefined(country))
            throw new DomainException("Country doesnt exist");
    }
}
