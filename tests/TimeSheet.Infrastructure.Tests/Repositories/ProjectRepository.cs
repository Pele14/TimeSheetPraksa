

using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Timesheet.Domain.Entities;
using TimeSheet.Domain.Enums;
using TimeSheet.Infrastructure.Repositories;
using TimeSheet.Infrastructure.Tests.Common;

namespace TimeSheet.Infrastructure.Tests.Repositories;





[TestFixture]
public class ProjectRepositoryTests
{
    private TestDatabase database=null!;

    [SetUp]
    public void SetUp()
    {
        database=new TestDatabase();

    }

    [TearDown]
    public void TearDown()
    {
        database.Dispose();
    }

    [Test]
    public async Task AddAsync_()
    {
        var (client, lead)=await AddParentsAsync();
        var project=await AddProjectAsync(client.Id, lead.Id);

        using(var context= database.CreateContext())
        {
            var repository=new ProjectRepository(context);
            var result=await repository.GetByIdAsync(project.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo(project.Name));
            Assert.That(result.Description, Is.EqualTo(project.Description));
            Assert.That(result.ProjectStatus, Is.EqualTo(ProjectStatus.Active));
            Assert.That(result.ClientId, Is.EqualTo(client.Id));
            Assert.That(result.TeamMemberId, Is.EqualTo(lead.Id));
        }

    }
    [Test]
    public async Task UpdateAsync_()
    {
        var(oldClient, oldLead)=await AddParentsAsync();
        var(newClient, newLead)=await AddParentsAsync("Marko", "marko@gmail.com", "marko123");
        var Project=await AddProjectAsync(oldClient.Id, oldLead.Id);

        using(var context = database.CreateContext())
        {
            var repository=new ProjectRepository(context);
            var loaded=await repository.GetByIdAsync(Project.Id);

            loaded!.Update("Novi naziv", "Novi opis", ProjectStatus.Inactive, newClient.Id, newLead.Id);
            await repository.UpdateAsync(loaded);
        }
        using(var context= database.CreateContext())
        {
            var result=await new ProjectRepository(context).GetByIdAsync(Project.Id);

            Assert.That(result!.Name, Is.EqualTo("Novi naziv"));
            Assert.That(result.Description, Is.EqualTo("Novi opis"));
            Assert.That(result.ProjectStatus, Is.EqualTo(ProjectStatus.Inactive));
            Assert.That(result.ClientId, Is.EqualTo(newClient.Id));
            Assert.That(result.TeamMemberId, Is.EqualTo(newLead.Id));
        }

    }

    [Test]
    public async Task DeleteAsync_()
    {
        var(client, lead)=await AddParentsAsync();
        var Project=await AddProjectAsync(client.Id, lead.Id);

        using(var context = database.CreateContext())
        {
            await new ProjectRepository(context).DeleteAsync(Project.Id);

        }
        using(var context = database.CreateContext())
        {
            var row=await context.Projects
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .SingleAsync(t=> t.Id==Project.Id);
            Assert.That(row.IsDeleted, Is.True);
            Assert.That(row.DeletedAt, Is.Not.Null);

            var result=await new ProjectRepository(context).GetByIdAsync(Project.Id);
            Assert.That(result, Is.Null);
        }
    }

    [Test]
    public async Task ExistsByNameAsync()
    {
        var (client, lead)= await AddParentsAsync();
        await AddProjectAsync(client.Id, lead.Id, "Project");

        using var context=database.CreateContext();
        var exists=await new ProjectRepository(context).ExistsByNameAsync(" PROJECT ");

        Assert.That(exists, Is.True);
    }

   /* [Test]
    public async Task HasTimeEntriesAsync()
    {

    }*/

    [Test]
    public async Task GetAllAsync_()
    {
        var(client, lead)= await AddParentsAsync();
        await AddProjectAsync(client.Id, lead.Id, "Project");
        await AddProjectAsync(client.Id, lead.Id, "Project 2");
        var deleted=await AddProjectAsync(client.Id, lead.Id, "Project 3");

        using(var context = database.CreateContext())
        {
            await new ProjectRepository(context).DeleteAsync(deleted.Id);
        }
        using(var context = database.CreateContext())
        {
            var result=await new ProjectRepository(context).GetAllAsync();

            Assert.That(result.Select(p=>p.Name), Is.EqualTo(new[]{"Project", "Project 2", }));

        }
    }


    [Test]
    public async Task HasNoTimeEntriesAsync_()
    {
        var(client, lead)=await AddParentsAsync();
        var Project=await AddProjectAsync(client.Id, lead.Id);

        using(var context = database.CreateContext())
        {
           var hasEntries= await new ProjectRepository(context).HasTimeEntriesAsync(Project.Id);

            Assert.That(hasEntries, Is.False);
        }
    }

    [Test]
    public async Task AddAsync_DuplicateName()
    {
        var(client, lead)=await AddParentsAsync();
        await AddProjectAsync(client.Id, lead.Id, "Project");

        using var context= database.CreateContext();
        var repository=new ProjectRepository(context);

        Assert.ThrowsAsync<DbUpdateException>(()=>
            repository.AddAsync(TestData.CreateProject(client.Id, lead.Id, "Project")));

    }




    private async Task<ProjectModel> AddProjectAsync(Guid clientid, Guid leadid, string name="VegaIT")
    {
        var project = TestData.CreateProject(clientid, leadid, name);

        using var context = database.CreateContext();
        await new ProjectRepository(context).AddAsync(project);

        return project;
    }
    private async Task<(ClientModel clientModel, TeamMemberModel teamMemberModel)>AddParentsAsync(
        string clientName="Aleksa",
        string email="aleksa.pele03@gmail.com",
        string username="aleksa123"
    )
    {
        var client=TestData.CreateClient(clientName);
        var lead=TestData.CreateMember(email, username);

        using var context=database.CreateContext();
        await new ClientRepository(context).AddAsync(client);
        await new TeamMemberRepository(context).AddAsync(lead);
            
        return (client, lead);
    }
}