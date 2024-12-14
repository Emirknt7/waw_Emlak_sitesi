using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAW.Models
{
    public class Favorite
    {
        [Key]
        public int FavoriteId { get; set; }

        // Foreign Keys
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [ForeignKey("Ad")]
        public int AdId { get; set; }

        // Navigation Properties
        public Customer Customer { get; set; }
        public Ad Ad { get; set; }
    }
}
