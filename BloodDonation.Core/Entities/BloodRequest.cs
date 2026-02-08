using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodDonation.Core.Entities;

public class BloodRequest
{
    public int Id { get; set; }

    public string HospitalId { get; set; }
    [ForeignKey("HospitalId")]
    public ApplicationUser? Hospital { get; set; }
    [Required]
    public int BloodTypeId { get; set; }
    public BloodType? BloodType { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "الكمية لازم تكون بين 1 و 100")]
    public int Quantity { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime RequestDate { get; set; } = DateTime.Now;
}