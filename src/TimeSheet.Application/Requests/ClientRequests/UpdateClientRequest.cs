using System.IO.Compression;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Requests.ClientRequests;


public class UpdateClientRequest
{
    public required string Name{get; set;}
    public required string Address{get; set;}
    public required string City{get; set;}
    public required string  Zip{get; set;}
    public required Country country{get; set;}

}