using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WAW.Data;
using WAW.Models;

namespace WAW.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Favorites()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = _context.Customers
                .Include(c => c.Favorites)
                .ThenInclude(f => f.Ad)
                .ThenInclude(ad => ad.Advertiser)
                .ThenInclude(a => a.User)
                .FirstOrDefault(c => c.UserId == userId);

            if (customer == null)
            {
                TempData["ErrorMessage"] = "Müşteri bulunamadı.";
                return RedirectToAction("Index", "Home");
            }

            var favoriteAds = customer.Favorites.Select(f => f.Ad).ToList();

            return View(favoriteAds);
        }
    }
}
