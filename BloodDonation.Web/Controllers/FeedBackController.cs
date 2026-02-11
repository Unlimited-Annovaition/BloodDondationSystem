using System.Security.Claims;
using BloodDonation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BloodDonation.Web.Controllers
{
    public class FeedBackController : Controller
    {
        private readonly AppDbContext _context;
        public FeedBackController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet] 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return RedirectToAction("Index", "Home");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
                var feedBack = new BloodDonation.Core.Entities.FeedBack
                {
                    Message = message,
                    UserId = userId
                };
                
                _context.FeedBacks.Add(feedBack);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
                
        }

    }
}
