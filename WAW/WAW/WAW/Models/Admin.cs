using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAW.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        // Navigation Properties
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
