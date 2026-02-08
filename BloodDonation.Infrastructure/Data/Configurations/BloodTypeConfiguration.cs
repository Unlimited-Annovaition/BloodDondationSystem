using BloodDonation.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloodDonation.Infrastructure.Data.Configurations;

public class BloodTypeConfiguration : IEntityTypeConfiguration<BloodType>
{
    public void Configure(EntityTypeBuilder<BloodType> builder)
    {
        builder.HasKey(b => b.Id);
            
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(5);

        builder.HasData(
            new BloodType { Id = 1, Name = "A+" },
            new BloodType { Id = 2, Name = "A-" },
            new BloodType { Id = 3, Name = "B+" },
            new BloodType { Id = 4, Name = "B-" },
            new BloodType { Id = 5, Name = "O+" },
            new BloodType { Id = 6, Name = "O-" },
            new BloodType { Id = 7, Name = "AB+" },
            new BloodType { Id = 8, Name = "AB-" }
        );
    }
}
