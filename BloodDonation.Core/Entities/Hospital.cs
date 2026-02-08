using System.ComponentModel.DataAnnotations;

namespace BloodDonation.Core.Entities;

public class Hospital
{
    
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Address { get; set; }
    
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}