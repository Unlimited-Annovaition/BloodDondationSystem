using BloodDonation.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloodDonation.Infrastructure.Data.Configurations
{
    public class BloodRequestConfiguration : IEntityTypeConfiguration<BloodRequest>
    {
        public void Configure(EntityTypeBuilder<BloodRequest> builder)
        {
            builder.HasKey(r => r.Id);
            
            builder.Property(r => r.Status)
                .HasDefaultValue("Pending"); // القيمة الافتراضية
        }
    }
}