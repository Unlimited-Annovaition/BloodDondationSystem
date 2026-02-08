using BloodDonation.Core.Entities;
using BloodDonation.Infrastructure.Data;
using BloodDonation.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Web.Controllers
{
    [Route("Hospitals")]
    public class HospitalsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; 
        
        public HospitalsController( AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateHospitalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true,
                    PhoneNumber = model.Phone
                };
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "Hospital");
                    var hospital = new Hospital
                    {
                        Name = model.Name,
                        Address = model.Address,
                        ApplicationUserId = newUser.Id
                    };
                    _context.Hospitals.Add(hospital);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
           return View(model);
        }
        
        
        
        [HttpGet]
        public IActionResult Create()
        {
            return View(); 
            
        }
        

    }
}
