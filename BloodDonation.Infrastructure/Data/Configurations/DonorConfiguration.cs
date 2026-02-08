using BloodDonation.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloodDonation.Infrastructure.Data.Configurations;

public class DonorConfiguration : IEntityTypeConfiguration<Donor>
{
    public void Configure(EntityTypeBuilder<Donor> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.FirstName)
            .IsRequired()
            .HasMaxLength(50); 

        builder.Property(d => d.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.NationalId)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(d => d.NationalId)
            .IsUnique();

        builder.HasOne(d => d.BloodType)
            .WithMany(b => b.Donors)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

