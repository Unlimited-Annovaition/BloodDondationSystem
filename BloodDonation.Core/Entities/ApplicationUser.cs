using Microsoft.AspNetCore.Identity;

namespace BloodDonation.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Donor? Donor { get; set; }
        public Hospital? Hospital { get; set; }
    }
}