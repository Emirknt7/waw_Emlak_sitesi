using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WAW.Data;
using WAW.Models;
using WAW.ViewModel;

namespace WAW.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return Login();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Kullanıcı bilgilerini oturuma kaydet
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("FullName", user.FirstName + " " + user.LastName);
                    HttpContext.Session.SetString("AccountType", GetAccountType(user.UserId));

                    ViewData["SuccessMessage"] = "Giriş başarılı!";
                    return RedirectToAction("Index", "Home");
                }

                // Hata mesajı ekle
                ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış.");
            }
            else
            {
                ViewData["ErrorMessage"] = "Lütfen gerekli bilgileri doldurun.";
            }
            return View(model);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet] 
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model, IFormFile AvatarFile)
        {
            if (ModelState.IsValid)
            {
                // تحقق من المستخدم
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Kullanıcı adı zaten kullanılıyor.");
                    return View(model);
                }

                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "E-posta adresi zaten kullanılıyor.");
                    return View(model);
                }

                // حفظ الصورة إذا تم رفعها
                string avatarUrl = null;
                if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(AvatarFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/avatars", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // ضمان وجود المجلد
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        AvatarFile.CopyTo(stream);
                    }

                    avatarUrl = $"/images/avatars/{fileName}";
                }

                // إنشاء المستخدم
                var user = new User
                {
                    Username = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password,
                    AvatarURL = avatarUrl,
                    Email = model.Email,
                    LastLoginDate = DateTime.Now
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                // ربط المستخدم مع نوع الحساب
                if (model.AccountType == "Müşteri")
                {
                    var customer = new Customer { UserId = user.UserId };
                    _context.Customers.Add(customer);
                }
                else if (model.AccountType == "Reklamcı")
                {
                    var advertiser = new Advertiser { UserId = user.UserId };
                    _context.Advertisers.Add(advertiser);
                }

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Hesabınız başarıyla kaydedildi! Şimdi giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }

            TempData["ErrorMessage"] = "Hesap kaydı başarısız oldu. Lütfen bilgilerinizi kontrol edin.";
            return View(model);
        }
        private string GetAccountType(int userId)
        {
            if (_context.Customers.Any(c => c.UserId == userId)) return "Müşteri";
            if (_context.Advertisers.Any(a => a.UserId == userId)) return "Reklamcı";
            return "Yönetici";
        }

        [HttpGet]
        public IActionResult Profile()
        {
            // الحصول على معرف المستخدم ونوع الحساب من الجلسة
            var userId = HttpContext.Session.GetInt32("UserId");
            var accountType = HttpContext.Session.GetString("AccountType");

            if (userId == null || accountType == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // جلب معلومات المستخدم من قاعدة البيانات
            var user = _context.Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // عرض صفحة الحساب مع بيانات المستخدم ونوع الحساب
            ViewData["AccountType"] = accountType;
            return View(user);
        }
        [HttpGet]
        public IActionResult EditAccount()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new EditProfileViewModel
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditAccount(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    if (_context.Users.Any(u => u.Username == model.Username && u.UserId != userId))
                    {
                        ModelState.AddModelError("Username", "Bu kullanıcı adı zaten kullanılıyor.");
                        return View(model);
                    }

                    // تحديث البيانات
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;


                    _context.SaveChanges();
                    ViewData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";
                    return RedirectToAction("Profile", "Account");
                }
            }

            ViewData["ErrorMessage"] = "Profil güncellenirken bir hata oluştu.";
            return View(model);
        }

    }

}
