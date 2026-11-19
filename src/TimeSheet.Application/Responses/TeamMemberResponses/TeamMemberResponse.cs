namespace TimeSheet.Application.Responses.TeamMemberResponses;


public class TeamMemberResponse
{
    public required Guid Id{get; set;}
    public required string Name{get; set;}
    public required string Username{get; set;}
    public required string Email{get; set;}
    public required double HoursPerWeek{get; set;}
    public required Role Role{get; set;}
    public required MemberStatus Status{get; set;}


}