using DicountGrpc.Models;
using Microsoft.EntityFrameworkCore;

namespace DicountGrpc.Data
{
    public class DiscountDbContext :DbContext
    {
        public DiscountDbContext(DbContextOptions<DiscountDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 1,
                ProductName = "IPhone 16",
                Description = "IPhone 16 Discount",
                Amount = 150
            },
             new Coupon { 
           
                Id = 2,
                ProductName = "Samsung S20",
                Description = "Samsung S20 Discount",
                Amount = 100
            });
        }
        public DbSet<Coupon> Coupons { get; set; }
    }
}
