namespace BloodDonation.Core.Entities;

public class Donation
{
    public int Id { get; set; }
    public int DonorId { get; set; }
    public Donor? Donor { get; set; } 
    public DateTime DonationDate { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Pending";
    public string? Notes { get; set; }
}