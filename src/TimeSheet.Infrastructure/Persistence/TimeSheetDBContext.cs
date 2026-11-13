using Microsoft.EntityFrameworkCore;
using Timesheet.Domain.Entities;



namespace TimeSheet.Infrastructure.Persistence;


    public class AppDbContext : DbContext
{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
         public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }


}