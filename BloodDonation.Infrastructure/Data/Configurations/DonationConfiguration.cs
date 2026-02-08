using BloodDonation.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloodDonation.Infrastructure.Data.Configurations
{
    public class DonationConfiguration : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.ToTable("Donations");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Pending"); 

            builder.Property(d => d.DonationDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(d => d.Notes)
                .HasMaxLength(500);

            builder
                .HasOne(d => d.Donor)          
                .WithMany(p => p.Donations)    
                .HasForeignKey(d => d.DonorId)  
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}