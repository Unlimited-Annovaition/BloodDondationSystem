using BloodDonation.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloodDonation.Infrastructure.Data.Configurations;

public class FeedBackConfiguration : IEntityTypeConfiguration<FeedBack>
{
    public void Configure(EntityTypeBuilder<FeedBack> builder)
    {
        builder.ToTable("FeedBacks");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder
            .HasOne(f => f.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}