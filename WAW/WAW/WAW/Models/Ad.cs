using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAW.Models
{
    public class Ad
    {
        [Key]
        public int AdId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageURL { get; set; } // سيتم رفع الصورة عند الإضافة.

        public string Address { get; set; }

        public decimal Price { get; set; }

        public string LocationURL { get; set; } // رابط الموقع الجغرافي.

        public DateTime DatePosted { get; set; } // تاريخ النشر.

        public bool Status { get; set; } // حالة الإعلان: نشط/غير نشط.

        // Foreign Key
        [ForeignKey("Advertiser")]
        public int AdvertiserId { get; set; }

        // Navigation Properties
        public Advertiser Advertiser { get; set; } // علاقة مع المعلن.

        public ICollection<Favorite> Favorites { get; set; } // علاقة مع المفضلات.
    }
}
