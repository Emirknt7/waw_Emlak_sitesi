using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAW.Models
{
    public class Advertiser
    {
        [Key]
        public int AdvertiserId { get; set; }

        // Navigation Properties
        // Users
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        // Ads
        public ICollection<Ad> Ads { get; set; }
        public ICollection<Conversation> Conversations { get; set; }
    }

}
