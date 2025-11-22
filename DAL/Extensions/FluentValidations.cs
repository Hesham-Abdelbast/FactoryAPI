using AppModels.Entities;
using AppModels.Entities.Employees;
using AppModels.Entities.Equipments;
using AppModels.Entities.MerchantMangement;
using AppModels.Entities.Store;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Extensions
{
    public static class FluentValidations
    {
        public static ModelBuilder AddFrameworkValidations(this ModelBuilder modelBuilder)
        {

            // تطبيق تلقائي لكل الـ entities اللي فيها IsDeleted
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var isDeletedProp = entityType.ClrType.GetProperty("IsDeleted");
                if (isDeletedProp != null && isDeletedProp.PropertyType == typeof(bool))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var prop = Expression.Property(parameter, "IsDeleted");
                    var filter = Expression.Lambda(
                        Expression.Equal(prop, Expression.Constant(false)),
                        parameter
                    );

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }

            // Configure unique constraints
            modelBuilder.Entity<MaterialType>()
                .HasIndex(m => m.Name)
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

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Warehouse)
                   .WithMany(w => w.Transactions)
                   .HasForeignKey(t => t.WarehouseId)
                   .OnDelete(DeleteBehavior.Cascade);


            return modelBuilder;

        }
    }
}
