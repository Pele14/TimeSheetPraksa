namespace TimeSheet.Application.Responses.AuthResponses;


public class LoginResponse
{
    public required string Token{get; set;}
    public required Guid Id{get; set;}
    public required string Name{get; set;}
    public required string Email{get; set;}

    public required Role Role{get; set;}


}