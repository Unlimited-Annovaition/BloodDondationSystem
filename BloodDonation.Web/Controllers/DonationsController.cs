using BloodDonation.Core.Entities;
using BloodDonation.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BloodDonation.Web.Controllers
{
   
    public class DonationsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonationsController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Donation model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var donor = await _context.Donors
                .FirstOrDefaultAsync(d => d.ApplicationUserId == user.Id);

            if (donor == null)
            {
                return NotFound("لم يتم العثور على ملف المتبرع الخاص بك.");
            }

            model.DonorId = donor.Id;         
            model.DonationDate = DateTime.Now; 
            model.Status = "Pending"; 
            
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Success));
            }
            return View(model);
        }
        
        
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var donor = await _context.Donors
                .FirstOrDefaultAsync(d => d.ApplicationUserId == userId);

            if (donor == null)
            {
                return View(new List<Donation>()); 
            }

            var donations = await _context.Donations
                .Where(d => d.DonorId == donor.Id) 
                .OrderByDescending(d => d.DonationDate) 
                .ToListAsync();

            return View(donations);
        }
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> PendingRequests()
        {
            var requests = await _context.Donations
                .Include(d => d.Donor)
                .Where(d => d.Status == "Pending")
                .OrderBy(d => d.DonationDate)
                .ToListAsync();

            return View(requests);
        }
       
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var donation = await _context.Donations
                .Include(d => d.Donor)
                .FirstOrDefaultAsync(d => d.Id == id);
    
            if (donation == null) return NotFound();

            donation.Status = "Approved";

            if (donation.Donor != null)
            {
              
                var typeId = donation.Donor.BloodTypeId; 

                var stock = await _context.BloodStocks
                    .FirstOrDefaultAsync(s => s.BloodTypeId == typeId);

                if (stock != null)
                {
                    stock.Quantity += 1;
                    _context.Update(stock);
                }
                else
                {
                    var newStock = new BloodStock
                    {
                        BloodTypeId = typeId,
                        Quantity = 1
                    };
                    _context.Add(newStock);
                }
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Donation Approved & Stock Updated!";
    
            return RedirectToAction(nameof(PendingRequests));
        }

       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
    
            if (donation == null)
            {
                return NotFound();
            }

            donation.Status = "Rejected"; 
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(PendingRequests));
        }
        public IActionResult Success()
        {
            return View();
        }
    }
}