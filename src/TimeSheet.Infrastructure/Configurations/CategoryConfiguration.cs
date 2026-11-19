using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timesheet.Infrastructure.DBEntities;
namespace TimeSheet.Infrastructure.Configurations;


public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.CreatedAt)
            .IsRequired();


        builder.HasMany(c => c.TimeEntries)
            .WithOne(te => te.Category)
            .HasForeignKey(te => te.CategoryId);

        builder.HasQueryFilter(c => !c.IsDeleted);

    }
}