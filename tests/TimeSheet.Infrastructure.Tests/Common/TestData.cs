using Timesheet.Domain.Entities;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Infrastructure.Tests.Common;


public static class TestData
{
    public static TeamMemberModel CreateMember(
        string email="aleksa.pele03@gmail.com",
        string username="aleksa123",
        string passwordHash="hashed-password")
    {
        return new TeamMemberModel(Guid.NewGuid(), "Aleksa", username, email, passwordHash, 40, Role.Admin, MemberStatus.Active);
    }

    public static ClientModel CreateClient(string name = "VegaIT")
    {
        return new ClientModel(Guid.NewGuid(), name, "Bulevar 1", "Novi Sad", "21000", Country.Serbia);

    }
    public static ProjectModel CreateProject(Guid clientId, Guid teamMemberId, string name = "Project")
    {
       return new ProjectModel(Guid.NewGuid(), name, "opis", ProjectStatus.Active, clientId, teamMemberId);

    }

}