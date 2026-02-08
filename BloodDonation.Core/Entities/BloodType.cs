namespace BloodDonation.Core.Entities;

public class BloodType
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public ICollection<Donor> Donors { get; set; }
}