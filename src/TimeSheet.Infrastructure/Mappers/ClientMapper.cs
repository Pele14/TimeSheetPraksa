using Timesheet.Domain.Entities;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Mappers;



public static class ClientMapper
{
    public static Client MapToEntity(this ClientModel client)

        => new Client
        {
            Id = client.Id,
            Name = client.Name,
            Address = client.Address,
            City = client.City,
            Zip = client.Zip,
            Country=client.Country

        };



    public static ClientModel MapToDomain(this Client client)

        => new ClientModel(client.Id, client.Name, client.Address, client.City, client.Zip, client.Country);

    public static void UpdateEntity(this ClientModel entity, Client client)
    {
        client.Name = entity.Name;
        client.Address = entity.Address;
        client.City = entity.City;
        client.Zip = entity.Zip;
        client.Country=entity.Country;

    }


}