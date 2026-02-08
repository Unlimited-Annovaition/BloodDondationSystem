using System.ComponentModel.DataAnnotations;

namespace BloodDonation.Core.Entities;

public class Donor
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string NationalId { get; set; } 
    public int BloodTypeId { get; set; }
    public BloodType BloodType { get; set; }
    public ICollection<Donation> Donations { get; set; }
    public string ApplicationUserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }
}