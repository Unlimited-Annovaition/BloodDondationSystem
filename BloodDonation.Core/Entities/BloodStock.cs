namespace BloodDonation.Core.Entities;

public class BloodStock
{
    public int Id { get; set; }
    public int BloodTypeId { get; set; } 
    public BloodType? BloodType { get; set; }
    public int Quantity { get; set; }
}