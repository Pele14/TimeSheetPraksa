using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SQLitePCL;
using Timesheet.Infrastructure.DBEntities;
namespace TimeSheet.Infrastructure.Configurations;


public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.ProjectStatus)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.HasOne(p => p.Client)
            .WithMany(c => c.Projects)
            .HasForeignKey(p=>p.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.TeamMember)
            .WithMany(tm => tm.Projects)
            .HasForeignKey(p => p.TeamMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(p => !p.IsDeleted);

    }
}