namespace TimeSheet.Application.TeamMember.DTOs;

public class CreateTeamMemberDTO
{
    
        public string? Name { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public double HoursPerWeek { get; set; }

        public Role Role { get; set; }

        public MemberStatus Status { get; set; }


}