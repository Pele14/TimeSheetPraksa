using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Configurations;

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("TeamMembers");

        builder.HasKey(tm => tm.Id);

        builder.Property(tm => tm.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(tm => tm.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(tm => tm.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tm => tm.PasswordHash)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(tm => tm.Role)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(tm => tm.Status)
            .HasConversion<string>()
            .IsRequired();


        builder.Property(tm => tm.CreatedAt)
            .IsRequired();
        
        builder.HasIndex(tm => tm.Email)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        builder.HasIndex(tm => tm.Username)
            .IsUnique()
            .HasFilter("IsDeleted = 0");

        builder.HasQueryFilter(tm => !tm.IsDeleted);

    }
}