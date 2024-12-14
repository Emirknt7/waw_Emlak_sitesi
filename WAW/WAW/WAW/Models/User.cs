using System.ComponentModel.DataAnnotations;

namespace WAW.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string AvatarURL { get; set; }
        public DateTime LastLoginDate { get; set; }

        // Navigation Properties
        public ICollection<Customer> Customers { get; set; }
        public ICollection<Admin> Admins { get; set; }
        public ICollection<Advertiser> Advertisers { get; set; }
    }
}
