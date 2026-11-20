using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Zip)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c=> c.Country)
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property(c => c.CreatedAt)
            .IsRequired();


        builder.HasQueryFilter(c => !c.IsDeleted);

    }
}