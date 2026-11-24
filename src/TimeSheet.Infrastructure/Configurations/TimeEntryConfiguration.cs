using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Configurations
{
    public class TimeEntryConfiguration : IEntityTypeConfiguration<TimeEntry>
    {
        public void Configure(EntityTypeBuilder<TimeEntry> builder)
        {
            builder.ToTable("TimeEntries");

            builder.HasKey(te => te.Id);

            builder.Property(te => te.Date)
                .IsRequired();

            builder.Property(te => te.Hours)
                .IsRequired();



            builder.Property(te => te.Overtime)
                .IsRequired();

            builder.Property(te => te.Description)
                .HasMaxLength(200);


            builder.Property(te => te.CreatedAt)
                .IsRequired();
            
            builder.HasOne(te => te.Project)
                .WithMany(p => p.TimeEntries)
                .HasForeignKey(te => te.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(te => te.Category)
                .WithMany(c => c.TimeEntries)
                .HasForeignKey(te => te.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(te => te.TeamMember)
                .WithMany(tm => tm.TimeEntries)
                .HasForeignKey(te => te.TeamMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(te => new { te.TeamMemberId, te.Date });
            
            builder.HasQueryFilter(te => !te.IsDeleted);



        }
    }
}