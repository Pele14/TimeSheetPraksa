
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Testing.Platform.Extensions.TestFramework;
using TimeSheet.Infrastructure.Persistence;


namespace TimeSheet.Infrastructure.Tests.Common;

public class TestDatabase: IDisposable
{
    private readonly SqliteConnection connection;
    private readonly DbContextOptions<AppDbContext> options;

    public TestDatabase()
    {
        connection=new SqliteConnection("DataSource=:memory:");
        connection.Open();

        options=new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

        using var context=CreateContext();
        context.Database.EnsureCreated();

    }
    public AppDbContext CreateContext()
    {
        return new AppDbContext(options);
    }

    public void Dispose()
    {
        connection.Dispose();
    }


}