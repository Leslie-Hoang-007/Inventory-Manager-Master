using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Inventory_Manager.Models;
using Microsoft.AspNetCore.Identity;

namespace Inventory_Manager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Inventory_Manager.Models.Orders>? Orders { get; set; }
        public DbSet<Inventory_Manager.Models.Products>? Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Orders>().HasData(
                new Orders
                {
                    Id = 4,
                    Date = DateTime.UtcNow.AddDays(-10),
                    Name = "Apple Ipad Pro 11",
                    Quantity = 10,
                    Paid = true,
                    Status = "OutForDelivery",
                    TotalPrice = 8000,
                },
                new Orders
                {
                    Id = 3,
                    Date = DateTime.UtcNow.AddDays(-50),
                    Name = "Apple Watch Series 4",
                    Quantity = 10,
                    Paid = true,
                    Status = "Arrived",
                    TotalPrice = 1600,
                },
                new Orders
                {
                    Id = 2,
                    Date = DateTime.UtcNow.AddDays(-100),
                    Name = "Microsoft Surface",
                    Quantity = 5,
                    Paid = true,
                    Status = "Arrived",
                    TotalPrice = 2600,
                },
                new Orders
                {
                    Id = 1,
                    Date = DateTime.UtcNow.AddDays(-150),
                    Name = "Google Pixel Phone",
                    Quantity = 23,
                    Paid = true,
                    Status = "Arrived",
                    TotalPrice = 9600,
                }
                );
            modelBuilder.Entity<Products>().HasData(
            new Products
            {
                Id = 3,
                Date = DateTime.UtcNow.AddDays(-50),
                Name = "Apple Watch Series 4",
                Quantity = 10,
                Paid = true,
                Status = "Arrived",
                TotalPrice = 1600,
            },
            new Products
            {
                Id = 2,
                Date = DateTime.UtcNow.AddDays(-100),
                Name = "Microsoft Surface",
                Quantity = 5,
                Paid = true,
                Status = "Arrived",
                TotalPrice = 2600,
            },
            new Products
            {
                Id = 1,
                Date = DateTime.UtcNow.AddDays(-150),
                Name = "Google Pixel Phone",
                Quantity = 23,
                Paid = true,
                Status = "Arrived",
                TotalPrice = 9600,
            }
            );

        }


    }
}