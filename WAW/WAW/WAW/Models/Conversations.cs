using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WAW.Views;

namespace WAW.Models
{
    public class Conversation
    {
        [Key]
        public int ConversationId { get; set; }
        public DateTime CreateDate { get; set; }
        
        // Foreign Keys
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [ForeignKey("Advertiser")]
        public int AdvertiserId { get; set; }

        // Navigation Properties
        public Customer Customer { get; set; }
        public Advertiser Advertiser { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
