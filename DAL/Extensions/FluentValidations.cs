using AppModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Extensions
{
    public static class FluentValidations
    {
        public static ModelBuilder AddFrameworkValidations(this ModelBuilder modelBuilder)
        {
            // Apply global query filters for soft delete
            modelBuilder.Entity<Merchant>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<MaterialType>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Transaction>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<StoreInventory>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Contact>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Warehouse>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<WarehouseInventory>().HasQueryFilter(e => !e.IsDeleted);


            // Configure unique constraints
            modelBuilder.Entity<MaterialType>()
                .HasIndex(m => m.Name)
                .IsUnique();

            modelBuilder.Entity<StoreInventory>()
                .HasIndex(s => s.MaterialTypeId)
                .IsUnique();

            // Configure relationships
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Merchant)
                .WithMany(m => m.Transactions)
                .HasForeignKey(t => t.MerchantId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.MaterialType)
                .WithMany(m => m.Transactions)
                .HasForeignKey(t => t.MaterialTypeId);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.AmountPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
               .Property(t => t.Type)
               .HasConversion<int>();

            modelBuilder.Entity<Transaction>()
                .Property(t => t.PricePerUnit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<StoreInventory>()
                .Property(s => s.CurrentQuantity)
                .HasPrecision(18, 2);

            modelBuilder.Entity<StoreInventory>()
                .HasOne(s => s.MaterialType)
                .WithOne(m => m.StoreInventory)
                .HasForeignKey<StoreInventory>(s => s.MaterialTypeId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Warehouse)
                   .WithMany(w => w.Transactions)
                   .HasForeignKey(t => t.WarehouseId)
                   .OnDelete(DeleteBehavior.Cascade);


            return modelBuilder;

        }
    }
}
