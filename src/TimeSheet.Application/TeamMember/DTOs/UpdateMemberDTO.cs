namespace TimeSheet.Application.TeamMember.DTOs;



public class UpdateTeamMemberDto
{
    public required string Name { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public double HoursPerWeek { get; set; }

    public Role Role { get; set; }

    public MemberStatus Status { get; set; }



}