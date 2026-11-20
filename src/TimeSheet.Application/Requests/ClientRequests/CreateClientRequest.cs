using System.IO.Compression;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Requests.ClientRequests;


public class CreateClientRequest
{
    public required string Name{get; set;}
    public required string Address{get; set;}
    public required string City{get; set;}
    public required string  Zip{get; set;}
    public required Country Country{get; set;}

}