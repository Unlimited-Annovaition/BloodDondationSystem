using System.ComponentModel.DataAnnotations;

namespace BloodDonation.Web.Models.ViewModels;

public class CreateHospitalViewModel
{
    [Required(ErrorMessage = "يرجى إدخال اسم المستشفى")]
    [Display(Name = "Hospital Name")]
    public string Name { get; set; }

    [Microsoft.Build.Framework.Required]
    public string Address { get; set; }

    [Phone]
    public string Phone { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
    public string ConfirmPassword { get; set; }
}
