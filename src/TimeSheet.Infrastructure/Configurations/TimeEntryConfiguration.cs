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

            builder.Property(te=>te.Date)
                .IsRequired();
                
            builder.Property(te => te.Time)
                .IsRequired();
                


            builder.Property(te => te.Overtime)
                .IsRequired();
                
            builder.Property(te => te.Description)
                .HasMaxLength(200);
            

            builder.Property(te => te.CreatedAt)
                .IsRequired();    


            builder.HasQueryFilter(te => !te.IsDeleted);    
                    

            
        }
    }
}