using Timesheet.Domain.Entities;
using TimeSheet.Application.Requests.ClientRequests;

namespace TimeSheet.Application.Responses.ClientResponses;
public static class ClientResponseMapper
{
    public static ClientResponse ToResponse(this ClientModel member) 
    {
        return new ClientResponse
        {
            Id=member.Id,
            Name=member.Name,
            Address=member.Address,
            City=member.City,
            Zip=member.Zip,
            Country=member.Country

        };

    }

}