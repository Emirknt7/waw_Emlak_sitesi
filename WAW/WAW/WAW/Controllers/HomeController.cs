using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WAW.Data;
using WAW.Models;
using WAW.ViewModel;

namespace WAW.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // En Yeni İlanlar
            var latestAds = _context.Ads
                .Include(a => a.Advertiser)
                    .ThenInclude(ad => ad.User) // Kullanıcı bilgilerini dahil et
                .Include(a => a.Favorites) // Favorileri dahil et
                    .ThenInclude(f => f.Customer) // Favorilerde müşteri bilgilerini dahil et
                .Where(a => a.Status) // Yalnızca aktif ilanlar
                .OrderByDescending(a => a.DatePosted) // Tarihe göre sırala
                .Take(3) // İlk 3 ilan
                .ToList();

            // En Çok Beğenilen İlanlar
            var mostLikedAds = _context.Ads
                .Include(a => a.Advertiser)
                    .ThenInclude(ad => ad.User)
                .Include(a => a.Favorites)
                    .ThenInclude(f => f.Customer)
                .Where(a => a.Status)
                .OrderByDescending(a => a.Favorites.Count) // Favori sayısına göre sırala
                .Take(3)
                .ToList();

            var model = new HomeIndexViewModel
            {
                LatestAds = latestAds,
                MostLikedAds = mostLikedAds
            };

            return View(model);
        }

        public IActionResult RealEstates()
        {
            // İlanları Favoriler ile birlikte getir
            var ads = _context.Ads
                .Include(a => a.Advertiser)
                .ThenInclude(ad => ad.User)
                .Include(a => a.Favorites)
                .ThenInclude(f => f.Customer)
                .Where(a => a.Status)
                .OrderByDescending(a => a.DatePosted)
                .ToList();

            return View(ads);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ToggleLike(int adId, string returnUrl)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Beğenmek için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            var customer = _context.Customers.FirstOrDefault(c => c.UserId == userId);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Müşteri bulunamadı.";
                return RedirectToAction("RealEstates");
            }

            var ad = _context.Ads.Include(a => a.Favorites).FirstOrDefault(a => a.AdId == adId);
            if (ad == null)
            {
                TempData["ErrorMessage"] = "İlan bulunamadı.";
                return RedirectToAction("RealEstates");
            }

            var favorite = ad.Favorites.FirstOrDefault(f => f.CustomerId == customer.CustomerId);
            if (favorite != null)
            {
                // Beğeniyi kaldır
                _context.Favorites.Remove(favorite);
                TempData["SuccessMessage"] = "Beğeni kaldırıldı.";
            }
            else
            {
                // Beğen ekle
                _context.Favorites.Add(new Favorite
                {
                    AdId = ad.AdId,
                    CustomerId = customer.CustomerId
                });
                TempData["SuccessMessage"] = "Beğenildi.";
            }

            _context.SaveChanges();
            return Redirect(returnUrl);
        }
    }
}
