using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAW.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Conversation> Conversations { get; set; }

    }
}
