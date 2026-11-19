using System.Security.Permissions;
using Timesheet.Domain.Entities;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Mappers;



public static class CountryMapper
{
    public static Country MapToEntity(this CountryEntity Country)

        => new Country
        {
            Id = Country.Id,
            Name = Country.Name,
            Code = Country.Code,

        };



    public static CountryEntity MapToDomain(this Country Country)

        => new CountryEntity(Country.Id, Country.Name, Country.Code);

    public static void UpdateEntity(this Country Country, CountryEntity entity)
    {
        Country.Name = entity.Name;
        Country.Code = entity.Code;
    }


}