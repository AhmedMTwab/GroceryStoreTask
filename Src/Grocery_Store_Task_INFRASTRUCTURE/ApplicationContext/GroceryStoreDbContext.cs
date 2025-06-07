using Grocery_Store_Task_DOMAIN.Enums;
using Grocery_Store_Task_DOMAIN.Models;
using Microsoft.EntityFrameworkCore;

namespace Grocery_Store_Task_INFRASTRUCTURE.ApplicationContext
{
    public class GroceryStoreDbContext : DbContext
    {
        public GroceryStoreDbContext(DbContextOptions options) : base(options)
        { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<TimeSlot> TimeSlots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region SeedingProductData
            modelBuilder.Entity<Product>().HasData(
         new Product
         {
             Id = Guid.Parse("a1b2c3d4-e5f6-7000-8000-000000000001"),
             Name = "Wireless Mouse",
             Description = "Ergonomic wireless mouse with adjustable DPI settings, perfect for office and casual gaming.",
             Category = ProductTypeEnum.InStock,
         },
           new Product
           {
               Id = Guid.Parse("a1b2c3d4-e5f6-7000-8000-000000000002"),
               Name = "Organic Apples",
               Description = "Freshly picked organic Gala apples, sweet and crisp, ideal for snacking or baking.",
               Category = ProductTypeEnum.FreshFood,
           },
           new Product
           {
               Id = Guid.Parse("a1b2c3d4-e5f6-7000-8000-000000000003"),
               Name = "Custom Leather Sofa",
               Description = "Handcrafted custom-made leather sofa, designed for comfort and durability. Lead time: 4 weeks.",
               Category = ProductTypeEnum.ExternalProduct,
           },
           new Product
           {
               Id = Guid.Parse("a1b2c3d4-e5f6-7000-8000-000000000004"),
               Name = "Bluetooth Keyboard",
               Description = "Compact and portable Bluetooth keyboard with multi-device connectivity, great for tablets and phones.",
               Category = ProductTypeEnum.InStock,
           },
           new Product
           {
               Id = Guid.Parse("a1b2c3d4-e5f6-7000-8000-000000000005"),
               Name = "Artisan Sourdough",
               Description = "Freshly baked artisan sourdough bread, made with natural yeast and slow fermentation for a rich flavor.",
               Category = ProductTypeEnum.FreshFood,
           }
      );
            #endregion


            base.OnModelCreating(modelBuilder);
        }
    }
}
