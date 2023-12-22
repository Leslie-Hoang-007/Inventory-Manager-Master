using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Inventory_Management.Models;
using Inventory_Manager.Models;
using Microsoft.AspNetCore.Identity;

namespace Inventory_Manager.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Inventory_Management.Models.Inventory>? Inventory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Inventory>().HasData(
                new Inventory
                {
                    Id = 3,
                    Date = DateTime.UtcNow.AddDays(-50),
                    Name = "Apple Watch Series 4",
                    Quantity = 10,
                    Paid = true,
                    Status = "Arrived",
                    TotalPrice = 1600,
                },
                new Inventory
                {
                    Id = 2,
                    Date = DateTime.UtcNow.AddDays(-100),
                    Name = "Microsoft Surface",
                    Quantity = 5,
                    Paid = true,
                    Status = "Arrived",
                    TotalPrice = 2600,
                },
                new Inventory
                {
                    Id = 1,
                    Date = DateTime.UtcNow.AddDays(-150),
                    Name = "Google Pixel Phone",
                    Quantity = 23,
                    Paid = true,
                    Status = "OutForDelivery",
                    TotalPrice = 9600,
                }
                );
        }

        public DbSet<Inventory_Manager.Models.Products>? Products { get; set; }
    }
}