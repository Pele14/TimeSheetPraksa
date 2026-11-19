namespace TimeSheet.Application.Requests.TeamMemberRequests;


public class UpdateTeamMemberRequest
{
    public required string Name{get; set;}
    public required string Username{get; set;}
    public required string Email{get; set;}
    public required double HoursPerWeek{get; set;}
    public required Role Role{get; set;}
    public required MemberStatus Status{get; set;}
}