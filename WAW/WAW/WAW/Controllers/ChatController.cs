using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WAW.Data;
using WAW.Models;
using WAW.Views;

namespace WAW.Controllers
{
    public class ChatController : Controller
    {
        private readonly AppDbContext _context;

        public ChatController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapınız.";
                return RedirectToAction("Login", "Account");
            }

            var accountType = HttpContext.Session.GetString("AccountType");
            IQueryable<Conversation> conversations;

            if (accountType == "Müşteri")
            {
                conversations = _context.Conversations
                    .Include(c => c.Advertiser)
                        .ThenInclude(a => a.User)
                    .Where(c => c.Customer.UserId == userId);
            }
            else if (accountType == "Reklamcı")
            {
                conversations = _context.Conversations
                    .Include(c => c.Customer)
                        .ThenInclude(cu => cu.User)
                    .Where(c => c.Advertiser.UserId == userId);
            }
            else
            {
                conversations = Enumerable.Empty<Conversation>().AsQueryable();
            }

            return View(conversations.ToList());
        }

        public IActionResult Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapınız.";
                return RedirectToAction("Login", "Account");
            }

            // تحميل المحادثة مع الرسائل والطرف الآخر
            var conversation = _context.Conversations
                .Include(c => c.Messages)
                .Include(c => c.Customer)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Advertiser)
                    .ThenInclude(ad => ad.User)
                .FirstOrDefault(c => c.ConversationId == id);

            if (conversation == null)
            {
                TempData["ErrorMessage"] = "Sohbet bulunamadı.";
                return RedirectToAction("Index");
            }

            return View(conversation);
        }


        [HttpPost]
        public IActionResult SendMessage(int conversationId, string content)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "Mesaj gönderilemedi.";
                return RedirectToAction("Details", new { id = conversationId });
            }

            var conversation = _context.Conversations.FirstOrDefault(c => c.ConversationId == conversationId);
            if (conversation == null)
            {
                TempData["ErrorMessage"] = "Sohbet bulunamadı.";
                return RedirectToAction("Index");
            }

            var senderType = HttpContext.Session.GetString("AccountType") == "Müşteri"
                ? SenderType.Customer
                : SenderType.Advertiser;

            var message = new Message
            {
                Content = content,
                DateSent = DateTime.Now,
                ConversationId = conversationId,
                SenderType = senderType
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = conversationId });
        }

        [HttpGet]
        public IActionResult StartChat(int advertiserId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapınız.";
                return RedirectToAction("Login", "Account");
            }

            // التحقق مما إذا كان المستخدم الحالي هو عميل
            var customer = _context.Customers.FirstOrDefault(c => c.UserId == userId);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Bu işlem yalnızca müşteriler içindir.";
                return RedirectToAction("RealEstates", "Home");
            }

            // تحقق مما إذا كانت هناك محادثة موجودة بالفعل
            var existingConversation = _context.Conversations
                .FirstOrDefault(c => c.CustomerId == customer.CustomerId && c.AdvertiserId == advertiserId);

            if (existingConversation != null)
            {
                // إذا كانت المحادثة موجودة، الانتقال مباشرة إلى تفاصيلها
                return RedirectToAction("Details", new { id = existingConversation.ConversationId });
            }

            // إنشاء محادثة جديدة
            var newConversation = new Conversation
            {
                CustomerId = customer.CustomerId,
                AdvertiserId = advertiserId,
                CreateDate = DateTime.Now
            };

            _context.Conversations.Add(newConversation);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = newConversation.ConversationId });
        }


    }
}
