using BloodDonation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodDonation.Web.Controllers;

public class BloodStockController : Controller
{
    private readonly AppDbContext _context;
    public BloodStockController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
       var stock = await _context.BloodStocks
            .Include(s => s.BloodType)
            .OrderBy(s=>s.BloodType.Name)
            .ToListAsync();
        return View(stock);
    }
    
}