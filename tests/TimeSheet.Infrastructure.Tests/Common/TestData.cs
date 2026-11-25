using Timesheet.Domain.Entities;
using Timesheet.Infrastructure.DBEntities;
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
    public static Category CreateCategory(string name = "Design")
    {
        return new Category{Id=Guid.NewGuid(), Name= name};
    }
    public static TimeEntryModel CreateTimeEntry(Guid projectId, Guid categoryId, Guid teamMemberId)
    {
        return new TimeEntryModel(
            Guid.NewGuid(),
            new DateOnly(2026, 11, 25),
            7.5,
            0.5,
            "Description",
            project: projectId,
            category: categoryId,
            teammember: teamMemberId
        );

    }

}