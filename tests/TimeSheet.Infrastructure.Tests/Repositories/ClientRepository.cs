

using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Timesheet.Domain.Entities;
using TimeSheet.Domain.Enums;
using TimeSheet.Infrastructure.Repositories;
using TimeSheet.Infrastructure.Tests.Common;

namespace TimeSheet.Infrastructure.Tests.Repositories;





[TestFixture]
public class ClientRepositoryTests
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
        var client=await AddClientAsync();

        using(var context= database.CreateContext())
        {
            var repository=new ClientRepository(context);
            var result=await repository.GetByIdAsync(client.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo(client.Name));
            Assert.That(result.Address, Is.EqualTo(client.Address));
            Assert.That(result.City, Is.EqualTo(client.City));
            Assert.That(result.Zip, Is.EqualTo(client.Zip));
            Assert.That(result.Country, Is.EqualTo(client.Country));
        }

    }
    [Test]
    public async Task UpdateAsync_()
    {
        
        var client=await AddClientAsync();

        using(var context = database.CreateContext())
        {
            var repository=new ClientRepository(context);
            var loaded=await repository.GetByIdAsync(client.Id);

            loaded!.Update("Continental", "Ilica 4", "Zagreb", "10000", Country.Croatia);
            await repository.UpdateAsync(loaded);
        }
        using(var context= database.CreateContext())
        {
            var result=await new ClientRepository(context).GetByIdAsync(client.Id);

            Assert.That(result!.Name, Is.EqualTo("Continental"));
            Assert.That(result.Address, Is.EqualTo("Ilica 4"));
            Assert.That(result.City, Is.EqualTo("Zagreb"));
            Assert.That(result.Zip, Is.EqualTo("10000"));
            Assert.That(result.Country, Is.EqualTo(Country.Croatia));
        }

    }

    [Test]
    public async Task DeleteAsync_()
    {
        var client=await AddClientAsync();

        using(var context = database.CreateContext())
        {
            await new ClientRepository(context).DeleteAsync(client.Id);

        }
        using(var context = database.CreateContext())
        {
            var row=await context.Clients
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .SingleAsync(t=> t.Id==client.Id);
            Assert.That(row.IsDeleted, Is.True);
            Assert.That(row.DeletedAt, Is.Not.Null);

            var result=await new ClientRepository(context).GetByIdAsync(client.Id);
            Assert.That(result, Is.Null);
        }
    }

    [Test]
    public async Task ExistsByNameAsync()
    {
        await AddClientAsync(name: "Continental");

        using var context=database.CreateContext();
        var exists=await new ClientRepository(context).ExistsByNameAsync(" CONTINENTAL ");

        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task HasNoProjectsAsync()
    {
        var client=await AddClientAsync();

        using var context=database.CreateContext();
        var hasProjects=await new ClientRepository(context).HasProjectAsync(client.Id);

        Assert.That(hasProjects, Is.False);

    }

    [Test]
    public async Task HasProjectsAsync_()
    {
        var client=await AddClientAsync();
        var lead=TestData.CreateMember();

        using(var context = database.CreateContext())
        {
            await new TeamMemberRepository(context).AddAsync(lead);
            await new ProjectRepository(context).AddAsync(TestData.CreateProject(client.Id, lead.Id));

        }
        using(var context = database.CreateContext())
        {
            var hasProjects=await new ClientRepository(context).HasProjectAsync(client.Id);

            Assert.That(hasProjects, Is.True);
        }
    }

    [Test]
    public async Task AddAsync_DuplicateName()
    {
        await AddClientAsync(name: "VegaIT");

        using var context= database.CreateContext();
        var repository=new ClientRepository(context);

        Assert.ThrowsAsync<DbUpdateException>(()=>
            repository.AddAsync(TestData.CreateClient(name: "VegaIT")));

    }




private async Task<ClientModel> AddClientAsync(string name="VegaIT")
{
    var client = TestData.CreateClient(name);

    using var context = database.CreateContext();
    await new ClientRepository(context).AddAsync(client);

    return client;
}

}