using Timesheet.Domain.Entities;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Mappers;



public static class ClientMapper
{
    public static Client MapToEntity(this ClientEntity client)
    
        => new Client
        {
            Id=client.Id,
            Name=client.Name,
            Address=client.Address,
            City=client.City,
            Zip=client.Zip,
            CountryId=client.CountryEntityId

        };



    public static ClientEntity MapToDomain(this Client client)
    
        => new ClientEntity(client.Id, client.Name, client.Address, client.City, client.Zip, client.CountryId);

    public static void UpdateEntity(this Client client, ClientEntity entity)
    {
        client.Name=entity.Name;
        client.Address=entity.Address;
        client.City=entity.City;
        client.Zip=entity.Zip;
        client.CountryId=client.CountryId;

    }


    }