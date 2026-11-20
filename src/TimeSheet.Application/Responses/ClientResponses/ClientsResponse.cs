using TimeSheet.Application.Requests.ClientRequests;

namespace TimeSheet.Application.Responses.ClientResponses;


public class ClientsResponse
{
    public required List<ClientResponse>Clients{get; set;}

}