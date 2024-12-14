using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WAW.Data;
using WAW.Models;

namespace WAW.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult NewAds()
        {
            // جلب جميع الإعلانات مع معلومات المعلن
            var ads = _context.Ads
                .Include(ad => ad.Advertiser)
                .ThenInclude(a => a.User)
                .OrderByDescending(ad => ad.DatePosted)
                .ToList();

            return View(ads);
        }

        public IActionResult AdDetails(int id)
        {
            // جلب الإعلان المحدد مع معلومات المعلن
            var ad = _context.Ads
                .Include(ad => ad.Advertiser)
                .ThenInclude(a => a.User)
                .FirstOrDefault(ad => ad.AdId == id);

            if (ad == null)
            {
                TempData["ErrorMessage"] = "İlan bulunamadı.";
                return RedirectToAction("NewAds");
            }

            return View(ad);
        }
        [HttpPost]
        public IActionResult ToggleStatus(int adId)
        {
            // جلب الإعلان بناءً على المعرف
            var ad = _context.Ads.FirstOrDefault(a => a.AdId == adId);
            if (ad == null)
            {
                TempData["ErrorMessage"] = "İlan bulunamadı.";
                return RedirectToAction("NewAds");
            }

            // تغيير حالة الإعلان
            ad.Status = !ad.Status;
            _context.SaveChanges();

            TempData["SuccessMessage"] = ad.Status
                ? "İlan aktifleştirildi."
                : "İlan pasifleştirildi.";

            return RedirectToAction("NewAds");
        }
    }
}
