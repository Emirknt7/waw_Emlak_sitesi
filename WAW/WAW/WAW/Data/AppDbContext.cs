using Microsoft.EntityFrameworkCore;
using WAW.Models;
using WAW.Views;

namespace WAW.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Customer)
                .WithMany(c => c.Favorites)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); 
            
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Ad)
                .WithMany(a => a.Favorites)
                .HasForeignKey(f => f.AdId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.Customer)
                .WithMany(cus => cus.Conversations)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.Advertiser)
                .WithMany(adv => adv.Conversations)
                .HasForeignKey(c => c.AdvertiserId)
                .OnDelete(DeleteBehavior.Restrict); 

        }
    }
}
