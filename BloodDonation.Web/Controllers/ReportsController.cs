
using BloodDonation.Core.Entities;
using BloodDonation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BloodDonation.Web.Controllers
{
    [Authorize(Roles = "Admin")] 
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
                        
            public async Task<IActionResult> Index()
            {
                var donors = await _userManager.GetUsersInRoleAsync("Donor");
                ViewBag.TotalDonors = donors.Count;

                var hospitals = await _userManager.GetUsersInRoleAsync("Hospital");
                ViewBag.TotalHospitals = hospitals.Count;

                var totalBloodUnits = await _context.BloodStocks.SumAsync(s => s.Quantity);
                ViewBag.TotalBloodUnits = totalBloodUnits;

                return View();
            }

        public async Task<IActionResult> DonationsReport(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Donations
                .Include(d => d.Donor) 
                .ThenInclude(d => d.ApplicationUser) 
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(d => d.DonationDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(d => d.DonationDate <= endDate.Value);
            }

            var result = await query.OrderByDescending(d => d.DonationDate).ToListAsync();

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(result);
        }

        public async Task<IActionResult> HospitalRequestsReport(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.BloodRequests
                .Include(r => r.Hospital)
                .Include(r => r.BloodType)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(r => r.RequestDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(r => r.RequestDate <= endDate.Value);
            }

            var result = await query.OrderByDescending(r => r.RequestDate).ToListAsync();

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(result);
        }
    }
}