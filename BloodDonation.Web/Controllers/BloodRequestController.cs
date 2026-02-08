using BloodDonation.Core.Entities;
using BloodDonation.Infrastructure.Data;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BloodDonation.Web.Controllers
{
    
    public class BloodRequestsController : Controller
    {
        private readonly AppDbContext _context;

        public BloodRequestsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
         
            ViewData["BloodTypeId"] = new SelectList(_context.BloodTypes, "Id", "Name"); 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Hospital")] 
        public async Task<IActionResult> Create([Bind("BloodTypeId,Quantity")] BloodRequest bloodRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bloodRequest.HospitalId = userId;
            
            ModelState.Remove("HospitalId");
            ModelState.Remove("Hospital");
            
            bloodRequest.Status = "Pending";
            bloodRequest.RequestDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(bloodRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }

            ViewData["BloodTypeId"] = new SelectList(_context.BloodTypes, "Id", "Name", bloodRequest.BloodTypeId);
            return View(bloodRequest);
        }
        [Authorize(Roles = "Hospital")]    
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var myRequests = await _context.BloodRequests
                .Include(b => b.BloodType) 
                .Where(b => b.HospitalId == userId)
                .OrderByDescending(b => b.RequestDate)
                .ToListAsync();

            return View(myRequests);
        }
        
        
// منطقة الآدمن (Admin Zone)         
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PendingRequests()
        {
            var requests = await _context.BloodRequests
                .Include(b => b.Hospital) 
                .Include(b => b.BloodType)
                .Where(b => b.Status == "Pending")
                .OrderBy(b => b.RequestDate)
                .ToListAsync();

            return View(requests);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var request = await _context.BloodRequests.FindAsync(id);
            if (request == null) return NotFound();

            var stock = await _context.BloodStocks
                .FirstOrDefaultAsync(s => s.BloodTypeId == request.BloodTypeId);

            if (stock == null || stock.Quantity < request.Quantity)
            {
                TempData["Error"] = "للاسف! الكمية المتوفرة في المخزون غير كافية لهذا الطلب.";
                return RedirectToAction(nameof(PendingRequests));
            }

            stock.Quantity -= request.Quantity;
            request.Status = "Approved";

            await _context.SaveChangesAsync();
    
            TempData["Success"] = "تمت الموافقة وخصم الكمية من المخزون بنجاح!";
            return RedirectToAction(nameof(PendingRequests));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectRequest(int id)
        {
            var request = await _context.BloodRequests.FindAsync(id);
            if (request == null) return NotFound();

            request.Status = "Rejected";
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم رفض الطلب.";
            return RedirectToAction(nameof(PendingRequests));
        }
    }
}