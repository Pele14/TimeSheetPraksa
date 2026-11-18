namespace TimeSheet.Application.TeamMember.DTOs;

public class CreateTeamMemberDTO
{
    
        public required string Name { get; set; }

        public required string Username { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public double HoursPerWeek { get; set; }

        public Role Role { get; set; }

        public MemberStatus Status { get; set; }


}